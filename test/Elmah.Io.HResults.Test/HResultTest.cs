﻿using NUnit.Framework;
using System.Globalization;

namespace Elmah.Io.HResults.Test
{
    public class HResultTest
    {
        [TestCase(-2003303418, "0x88980406", 2200, "FACILITY_WINCODEC_DWRITE_DWM", 1030, "UCEERR_RENDERTHREADFAILURE")]
        [TestCase(-2147024894, "0x80070002", 7, "FACILITY_WIN32", 2, "ERROR_FILE_NOT_FOUND")]
        [TestCase(-2146232832, "0x80131600", 19, "FACILITY_URT", 5632, "COR_E_APPLICATION")]
        [TestCase(-2146828283, "0x800A0005", 10, "FACILITY_CONTROL", 5, "CTL_E_ILLEGALFUNCTIONCALL")]
        public void CanParseKnownFailure(int hresult, string expectedHex, int expectedFacilityCode, string expectedFacilityName, int expectedCode, string expectedName)
        {
            var res = HResult.Parse(hresult);
            Assert.IsNotNull(res);
            Assert.That(res.Hex, Is.EqualTo(expectedHex));
            Assert.That(res.IsFailure, Is.True);
            Assert.That(res.Facility, Is.Not.Null);
            Assert.That(res.Facility.Identifier, Is.EqualTo(expectedFacilityCode));
            Assert.That(res.Facility.Name, Is.EqualTo(expectedFacilityName));
            Assert.That(res.Facility.IsMatch, Is.True);
            Assert.That(res.Code, Is.Not.Null);
            Assert.That(res.Code.Identifier, Is.EqualTo(expectedCode));
            Assert.That(res.Code.Name, Is.EqualTo(expectedName));
            Assert.That(res.Code.IsMatch, Is.True);
        }

        [Test]
        public void CanParseUnknown()
        {
            var res = HResult.Parse(-2137124863);
            Assert.IsNotNull(res);
            Assert.That(res.Hex, Is.EqualTo("0x809E1001"));
            Assert.That(res.IsFailure, Is.True);
            Assert.That(res.Facility, Is.Not.Null);
            Assert.That(res.Facility.Identifier, Is.EqualTo(158));
            Assert.That(res.Facility.Name, Is.EqualTo("158"));
            Assert.That(res.Facility.IsMatch, Is.False);
            Assert.That(res.Code, Is.Not.Null);
            Assert.That(res.Code.Identifier, Is.EqualTo(4097));
            Assert.That(res.Code.Name, Is.EqualTo("4097"));
            Assert.That(res.Code.IsMatch, Is.False);
        }

        [Test]
        public void CanParseSuccess()
        {
            var p = int.Parse("00040000", NumberStyles.HexNumber);
            var res = HResult.Parse(p);
            Assert.That(res.IsFailure, Is.False);
        }
    }
}