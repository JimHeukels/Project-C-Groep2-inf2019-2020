using System;
using Xunit;
using Bliss_Programma.Services;

namespace Bliss_Programma_Tests
{
    public class Services_Test
    {
        [Fact]
        public void Prio_Test()
        {
            Assert.True(Functies.Prio("2") == 14);
        }
        [Fact]
        public void Prio_Test2()
        {
            Assert.Throws<System.FormatException>( ()=> Functies.Prio("a") == 14);
        }
        [Fact]
        public void maxbezetting_Test()
        {
            Assert.True(Functies.maxbezetting(21) == 11);
        }
    }
}
