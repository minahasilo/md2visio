using md2visio.figure;
using md2visio.graph;
using Microsoft.Office.Interop.Visio;

namespace md2visio.vsdx
{
    internal class VBoundary: IEmpty
    {
        public static readonly VBoundary Empty = new VBoundary();

        protected double pinx, piny, width, height;
        public double PinX { get { return pinx; } }
        public double PinY { get { return piny; } }
        public double Width { get { return width; } }  
        public double Height { get { return height; } }
        public double Left { 
            get { return pinx - width / 2; } 
            set {
                double r = Right;
                if (value > r) return;                
                pinx = (value + r) / 2;
                width = r - value;
            } 
        }

        public double Right { 
            get { return pinx + width / 2; } 
            set {
                double L = Left;
                if(value < L) return;
                pinx = (L + value) / 2;
                width = value - L;
            }
        }

        public double Top { 
            get { return piny + height / 2; }
            set {
                double b = Bottom;
                if (value < b) return;
                piny = (b + value) / 2;
                height = value - b;
            }
        }
        public double Bottom { 
            get { return piny - height / 2; }
            set {
                double t = Top;
                if(value > t) return;
                piny = (value + t) / 2;
                height = t - value;
            }
        }
        public VBoundary() { }

        public VBoundary(double pinx, double piny, double width, double height)
        {
            this.pinx = pinx;
            this.piny = piny;
            this.width = width;
            this.height = height;
        }

        public void AlignLeft(double left)
        {
            pinx = left + width / 2;
        }

        public void AlignTop(double top)
        {
            piny = top - height / 2;
        }

        public void AlignLeftTop(double left, double top)
        {
            AlignLeft(left);
            AlignTop(top);
        }

        public void AlignRight(double right)
        {
            pinx = right - width / 2;
        }

        public void AlignBottom(double bottom)
        {
            piny = bottom + height / 2;
        }

        public void AlignRightBottom(double right, double bottom)
        {
            AlignRight(right);
            AlignBottom(bottom);
        }

        public VBoundary Clone()
        {
            VBoundary clone = new VBoundary();
            clone.pinx = pinx;
            clone.piny = piny;  
            clone.width = width;
            clone.height = height;
            return clone;
        }

        public static VBoundary Create(LinkedList<GNode> nodes)
        {
            VBoundary boundary = Empty;
            foreach (var node in nodes)
            {
                Shape? shape = node.VisioShape;
                if (shape == null) continue;

                VBoundary bnd2cmp = new VShapeBoundary(shape);
                if (boundary == Empty) { boundary = bnd2cmp; continue; }
                
                ExpandBoundary(boundary, bnd2cmp);
            }

            return boundary;
        }

        public void Expand(VBoundary boundary)
        {
            ExpandBoundary(this, boundary);
        }

        public void Expand(Shape shape)
        {
            Expand(new VShapeBoundary(shape));
        }

        public static void ExpandBoundary(VBoundary bnd2expand, VBoundary bndCompare)
        {
            if(bndCompare == Empty) return;

            if (bndCompare.Left < bnd2expand.Left) bnd2expand.Left = bndCompare.Left;
            if (bndCompare.Right > bnd2expand.Right) bnd2expand.Right = bndCompare.Right;
            if (bndCompare.Top > bnd2expand.Top) bnd2expand.Top = bndCompare.Top;
            if (bndCompare.Bottom < bnd2expand.Bottom) bnd2expand.Bottom = bndCompare.Bottom;
        }

        public bool IsEmpty()
        {
            return this == Empty;
        }
    }
}
