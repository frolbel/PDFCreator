﻿using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using NUnit.Framework;
using pdfforge.PDFCreator.Core.Ghostscript;
using pdfforge.PDFCreator.Core.Ghostscript.OutputDevices;
using pdfforge.PDFCreator.Core.Settings.Enums;

namespace PDFCreator.Core.UnitTest.Ghostscript.OutputDevices
{
    class JpegDeviceParametersTest
    {

        private OutputDevice _jpegDevice;
        private Collection<string> _parameterStrings;
        private GhostscriptVersion _ghostscriptVersion;

        [SetUp]
        public void SetUp()
        {
            _jpegDevice = ParametersTestHelper.GenerateDevice(OutputFormat.Jpeg);
            _ghostscriptVersion = ParametersTestHelper.GhostscriptVersionDummie;
        }

        [Test]
        public void CheckDeviceIndependentDefaultParameters()
        {
            _parameterStrings = new Collection<string>(_jpegDevice.GetGhostScriptParameters(_ghostscriptVersion));
            ParametersTestHelper.CheckDefaultParameters(_parameterStrings);
        }

        [Test]
        public void CheckDeviceSpecificDefaultParameters()
        {
            _parameterStrings = new Collection<string>(_jpegDevice.GetGhostScriptParameters(_ghostscriptVersion));

            string jpegQualityParameter = "-dJPEGQ=" + _jpegDevice.Job.Profile.JpegSettings.Quality;
            Assert.Contains(jpegQualityParameter, _parameterStrings, "Missing default device parameter.");

            string dpiParameter = "-r" + _jpegDevice.Job.Profile.JpegSettings.Dpi;
            Assert.Contains(dpiParameter, _parameterStrings, "Missing default device parameter.");

            Assert.Contains("-dTextAlphaBits=4", _parameterStrings, "Missing default device parameter.");
            Assert.Contains("-dGraphicsAlphaBits=4", _parameterStrings, "Missing default device parameter.");

            var outputFileParameter = _parameterStrings.First(x => x.StartsWith("-sOutputFile="));
            Assert.IsNotNull(outputFileParameter, "Missing -sOutputFile parameter.");
            outputFileParameter = outputFileParameter.Replace("-sOutputFile=", "");
            var requiredOutput = Path.Combine(_jpegDevice.Job.JobTempOutputFolder, _jpegDevice.Job.JobTempFileName) + "%d.jpg";
            Assert.AreEqual(requiredOutput, outputFileParameter, "Failure in output");
        }

        [Test]
        public void ParametersTest_Color24Bit_Quality4_4Dpi()
        {
            _jpegDevice.Job.Profile.JpegSettings.Color = JpegColor.Color24Bit;
            _jpegDevice.Job.Profile.JpegSettings.Quality = 4;
            _jpegDevice.Job.Profile.JpegSettings.Dpi = 4;

            _parameterStrings = new Collection<string>(_jpegDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=jpeg", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_Color24Bit_Quality5_5Dpi()
        {
            _jpegDevice.Job.Profile.JpegSettings.Color = JpegColor.Color24Bit;
            _jpegDevice.Job.Profile.JpegSettings.Quality = 5;
            _jpegDevice.Job.Profile.JpegSettings.Dpi = 5;

            _parameterStrings = new Collection<string>(_jpegDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=jpeg", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_ColorGray_Quality100_2400Dpi()
        {
            _jpegDevice.Job.Profile.JpegSettings.Color = JpegColor.Gray8Bit;
            _jpegDevice.Job.Profile.JpegSettings.Quality = 100;
            _jpegDevice.Job.Profile.JpegSettings.Dpi = 2400;

            _parameterStrings = new Collection<string>(_jpegDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=jpeggray", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_ColorGray_Quality101_2401Dpi()
        {
            _jpegDevice.Job.Profile.JpegSettings.Color = JpegColor.Gray8Bit;
            _jpegDevice.Job.Profile.JpegSettings.Quality = 101;
            _jpegDevice.Job.Profile.JpegSettings.Dpi = 2401;

            _parameterStrings = new Collection<string>(_jpegDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=jpeggray", _parameterStrings, "Missing parameter.");
        }
    }
}

