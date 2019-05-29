﻿namespace OffUploader.Core
{
    using ImageMagick;
    using MediatR;
    using OffUploader.Core.Logging;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO.Abstractions;
    using System.Threading;
    using System.Threading.Tasks;
    using ZXing;
    using ZXing.Magick;

    public class ParseBarcodesAndUploadFilesHandler : IRequestHandler<ParseBarcodesAndUploadFiles>
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();

        private readonly IBarcodeReader barcodeReader;

        private readonly IFileSystem fileSystem;

        private readonly IMagickFactory magickFactory;

        private readonly IMediator mediator;

        public ParseBarcodesAndUploadFilesHandler(IBarcodeReader barcodeReader, IFileSystem fileSystem, IMagickFactory magickFactory, IMediator mediator)
        {
            this.barcodeReader = barcodeReader;
            this.fileSystem = fileSystem;
            this.magickFactory = magickFactory;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(ParseBarcodesAndUploadFiles request, CancellationToken cancellationToken)
        {
            var jpgs = request.Paths;
            string? code = null;
            log.Info("Reading barcodes from JPEGs: {@Paths}", jpgs);
            var stopwatch = Stopwatch.StartNew();
            foreach (var file in jpgs)
            {
                var barcodes = await this.ReadBarcodesAsync(file, cancellationToken).ConfigureAwait(false);
                if (barcodes == null)
                {
                    continue;
                }

                if (TryGetFirstCode(barcodes, out code))
                {
                    break;
                }
            }

            stopwatch.Stop();

            if (code == null)
            {
                log.Warn("Barcodes read from JPEGs in {@Paths} in {Duration}. No good barcode found for any file.", jpgs, stopwatch.Elapsed);
                throw new InvalidOperationException("No barcode found in the images.");
            }
            else
            {
                log.Info("Barcodes read from JPEGs in {@Paths} in {Duration}. Code is {Code}.", jpgs, stopwatch.Elapsed, code);
            }

            await this.mediator.Send(new UploadFilesToCodeRequest(request.Settings, code, jpgs), cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }

        private static bool TryGetFirstCode(IEnumerable<Result> barcodes, out string code)
        {
            code = string.Empty;

            foreach (var barcode in barcodes)
            {
                if (barcode.BarcodeFormat == BarcodeFormat.EAN_13 ||
                    barcode.BarcodeFormat == BarcodeFormat.EAN_8 ||
                    barcode.BarcodeFormat == BarcodeFormat.UPC_A ||
                    barcode.BarcodeFormat == BarcodeFormat.UPC_E)
                {
                    code = barcode.Text;
                    log.Info("Found a good barcode for the files: {Barcode}", code);
                    return true;
                }
            }

            return false;
        }

        private Task<Result[]> ReadBarcodesAsync(string path, CancellationToken cancellationToken) => Task.Run(() => this.ReadBarcodes(path), cancellationToken);

        private Result[] ReadBarcodes(string path)
        {
            log.Trace("Opening image {File} with ImageMagick", path);
            using var image = this.magickFactory.CreateImage(path);
            log.Trace("Reading barcode of image {File}", path);
            var luminanceSource = new MagickImageLuminanceSource((MagickImage)image);
            var barcodes = this.barcodeReader.DecodeMultiple(luminanceSource);
            log.Debug("Barcodes for {File} are {@Barcodes}", path, barcodes);
            return barcodes;
        }
    }
}