using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteBot.Core.Model
{
    public record Clipping(string BookTitle, string Author, string Content, DateTime CreationDate);
}