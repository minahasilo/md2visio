using md2visio.graph;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md2visio.figure
{
    internal interface INode
    {
        string ID { get; set; }
        string Label { get; set; }
        Shape? VisioShape { get; set; }
        Container Container { get; set; }
        List<GEdge> InputEdges { get; }
        List<GEdge> OutputEdges { get; }
    }
}
