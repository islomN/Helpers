using Helpers;
using Xunit;

namespace UnitTests
{
    public class TinHelperTests
    {
        [Theory]
        [InlineData("300023123")]
        [InlineData("523123123")]
        [InlineData("223123123")]
        [InlineData("423123123")]
        [InlineData("623000000")]
        public void IsTin_Validity(string tin)
        {
            var result = TinHelper.IsTin(tin);
            Assert.True(result);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("1asdasda")]
        [InlineData("12312312")]
        [InlineData("92312312")]
        [InlineData("92312312123")]
        [InlineData("22312312123")]
        public void IsTin_NotValidity(string tin)
        {
            var result = TinHelper.IsTin(tin);
            Assert.False(result);
        }
        
        [Theory]
        [InlineData("523123123")]
        [InlineData("623123999")]
        [InlineData("600000000")]
        public void IsIndividualTin_Validity(string tin)
        {
            var result = TinHelper.IsIndividualTin(tin);
            Assert.True(result);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("1asdasda")]
        [InlineData("300023123")]
        [InlineData("200023123")]
        [InlineData("100023123")]
        [InlineData("900023123")]
        [InlineData("90002312312")]
        [InlineData("800000000")]
        [InlineData("700000000")]
        public void IsIndividualTin_NotValidity(string tin)
        {
            var result = TinHelper.IsIndividualTin(tin);
            Assert.False(result);
        }
        
        [Theory]
        [InlineData("300023123")]
        [InlineData("200023123")]
        [InlineData("300000000")]
        public void IsLegalTin_Validity(string tin)
        {
            var result = TinHelper.IsLegalTin(tin);
            Assert.True(result);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("123123123")]
        [InlineData("12312312322")]
        [InlineData("923123123")]
        [InlineData("823123123")]
        [InlineData("523123123")]
        [InlineData("623123999")]
        public void IsLegalTin_NotValidity(string tin)
        {
            var result = TinHelper.IsLegalTin(tin);
            Assert.False(result);
        }
    }
}