using Microsoft.Office.Interop.Visio;
using stdole;
using System.Runtime.CompilerServices;

namespace md2visio.vsdx.@base
{
    internal class EmptyShape : Shape
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
        public EmptyShape() { }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。

        public void VoidGroup()
        {
            throw new NotImplementedException();
        }

        public void BringForward()
        {
            throw new NotImplementedException();
        }

        public void BringToFront()
        {
            throw new NotImplementedException();
        }

        public void ConvertToGroup()
        {
            throw new NotImplementedException();
        }

        public void FlipHorizontal()
        {
            throw new NotImplementedException();
        }

        public void FlipVertical()
        {
            throw new NotImplementedException();
        }

        public void ReverseEnds()
        {
            throw new NotImplementedException();
        }

        public void SendBackward()
        {
            throw new NotImplementedException();
        }

        public void SendToBack()
        {
            throw new NotImplementedException();
        }

        public void Rotate90()
        {
            throw new NotImplementedException();
        }

        public void Ungroup()
        {
            throw new NotImplementedException();
        }

        public void old_Copy()
        {
            throw new NotImplementedException();
        }

        public void old_Cut()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void VoidDuplicate()
        {
            throw new NotImplementedException();
        }

        public Shape Drop(object ObjectToDrop, double xPos, double yPos)
        {
            throw new NotImplementedException();
        }

        public short AddSection(short Section)
        {
            throw new NotImplementedException();
        }

        public void DeleteSection(short Section)
        {
            throw new NotImplementedException();
        }

        public short AddRow(short Section, short Row, short RowTag)
        {
            throw new NotImplementedException();
        }

        public void DeleteRow(short Section, short Row)
        {
            throw new NotImplementedException();
        }

        public void SetCenter(double xPos, double yPos)
        {
            throw new NotImplementedException();
        }

        public void SetBegin(double xPos, double yPos)
        {
            throw new NotImplementedException();
        }

        public void SetEnd(double xPos, double yPos)
        {
            throw new NotImplementedException();
        }

        public void Export(string FileName)
        {
            throw new NotImplementedException();
        }

        public short AddNamedRow(short Section, string RowName, short RowTag)
        {
            throw new NotImplementedException();
        }

        public short AddRows(short Section, short Row, short RowTag, short RowCount)
        {
            throw new NotImplementedException();
        }

        public Shape DrawLine(double xBegin, double yBegin, double xEnd, double yEnd)
        {
            throw new NotImplementedException();
        }

        public Shape DrawRectangle(double x1, double y1, double x2, double y2)
        {
            throw new NotImplementedException();
        }

        public Shape DrawOval(double x1, double y1, double x2, double y2)
        {
            throw new NotImplementedException();
        }

        public Shape DrawSpline(ref Array xyArray, double Tolerance, short Flags)
        {
            throw new NotImplementedException();
        }

        public Shape DrawBezier(ref Array xyArray, short degree, short Flags)
        {
            throw new NotImplementedException();
        }

        public Shape DrawPolyline(ref Array xyArray, short Flags)
        {
            throw new NotImplementedException();
        }

        public void FitCurve(double Tolerance, short Flags)
        {
            throw new NotImplementedException();
        }

        public Shape Import(string FileName)
        {
            throw new NotImplementedException();
        }

        public void CenterDrawing()
        {
            throw new NotImplementedException();
        }

        public Shape InsertFromFile(string FileName, short Flags)
        {
            throw new NotImplementedException();
        }

        public Shape InsertObject(string ClassOrProgID, short Flags)
        {
            throw new NotImplementedException();
        }

        public Window OpenDrawWindow()
        {
            throw new NotImplementedException();
        }

        public Window OpenSheetWindow()
        {
            throw new NotImplementedException();
        }

        public short DropMany(ref Array ObjectsToInstance, ref Array xyArray, out Array IDArray)
        {
            throw new NotImplementedException();
        }

        public void GetFormulas(ref Array SRCStream, out Array formulaArray)
        {
            throw new NotImplementedException();
        }

        public void GetResults(ref Array SRCStream, short Flags, ref Array UnitsNamesOrCodes, out Array resultArray)
        {
            throw new NotImplementedException();
        }

        public short SetFormulas(ref Array SRCStream, ref Array formulaArray, short Flags)
        {
            throw new NotImplementedException();
        }

        public short SetResults(ref Array SRCStream, ref Array UnitsNamesOrCodes, ref Array resultArray, short Flags)
        {
            throw new NotImplementedException();
        }

        public void Layout()
        {
            throw new NotImplementedException();
        }

        public void BoundingBox(short Flags, out double lpr8Left, out double lpr8Bottom, out double lpr8Right, out double lpr8Top)
        {
            throw new NotImplementedException();
        }

        public short HitTest(double xPos, double yPos, double Tolerance)
        {
            throw new NotImplementedException();
        }

        public Hyperlink AddHyperlink()
        {
            throw new NotImplementedException();
        }

        public void TransformXYTo(Shape OtherShape, double x, double y, out double xprime, out double yprime)
        {
            throw new NotImplementedException();
        }

        public void TransformXYFrom(Shape OtherShape, double x, double y, out double xprime, out double yprime)
        {
            throw new NotImplementedException();
        }

        public void XYToPage(double x, double y, out double xprime, out double yprime)
        {
            throw new NotImplementedException();
        }

        public void XYFromPage(double x, double y, out double xprime, out double yprime)
        {
            throw new NotImplementedException();
        }

        public void UpdateAlignmentBox()
        {
            throw new NotImplementedException();
        }

        public short DropManyU(ref Array ObjectsToInstance, ref Array xyArray, out Array IDArray)
        {
            throw new NotImplementedException();
        }

        public void GetFormulasU(ref Array SRCStream, out Array formulaArray)
        {
            throw new NotImplementedException();
        }

        public Shape DrawNURBS(short degree, short Flags, ref Array xyArray, ref Array knots, object weights)
        {
            throw new NotImplementedException();
        }

        public Shape Group()
        {
            throw new NotImplementedException();
        }

        public Shape Duplicate()
        {
            throw new NotImplementedException();
        }

        public void SwapEnds()
        {
            throw new NotImplementedException();
        }

        public void Copy(object Flags)
        {
            throw new NotImplementedException();
        }

        public void Cut(object Flags)
        {
            throw new NotImplementedException();
        }

        public void Paste(object Flags)
        {
            throw new NotImplementedException();
        }

        public void PasteSpecial(int Format, object Link, object DisplayAsIcon)
        {
            throw new NotImplementedException();
        }
#pragma warning disable CS8625
        public Selection CreateSelection(VisSelectionTypes SelType, VisSelectMode IterationMode = VisSelectMode.visSelModeSkipSuper, object Data = null)
        {
            throw new NotImplementedException();
        }
#pragma warning restore

        public void Offset(double Distance)
        {
            throw new NotImplementedException();
        }

        public Shape AddGuide(short Type, double xPos, double yPos)
        {
            throw new NotImplementedException();
        }

        public Shape DrawArcByThreePoints(double xBegin, double yBegin, double xEnd, double yEnd, double xControl, double yControl)
        {
            throw new NotImplementedException();
        }

        public Shape DrawQuarterArc(double xBegin, double yBegin, double xEnd, double yEnd, VisArcSweepFlags SweepFlag)
        {
            throw new NotImplementedException();
        }

        public Shape DrawCircularArc(double xCenter, double yCenter, double Radius, double StartAngle = 0, double EndAngle = 3.1415927410125732)
        {
            throw new NotImplementedException();
        }

        public void LinkToData(int DataRecordsetID, int RowID, bool ApplyDataGraphicAfterLink = true)
        {
            throw new NotImplementedException();
        }

        public void BreakLinkToData(int DataRecordsetID)
        {
            throw new NotImplementedException();
        }

        public int GetLinkedDataRow(int DataRecordsetID)
        {
            throw new NotImplementedException();
        }

        public void GetLinkedDataRecordsetIDs(out Array DataRecordsetIDs)
        {
            throw new NotImplementedException();
        }

        public void GetCustomPropertiesLinkedToData(int DataRecordsetID, out Array CustomPropertyIndices)
        {
            throw new NotImplementedException();
        }

        public bool IsCustomPropertyLinked(int DataRecordsetID, int CustomPropertyIndex)
        {
            throw new NotImplementedException();
        }

        public string GetCustomPropertyLinkedColumn(int DataRecordsetID, int CustomPropertyIndex)
        {
            throw new NotImplementedException();
        }

        public void AutoConnect(Shape ToShape, VisAutoConnectDir PlacementDir, [IUnknownConstant] object Connector)
        {
            throw new NotImplementedException();
        }

        public bool HasCategory(string Category)
        {
            throw new NotImplementedException();
        }

        public Array ConnectedShapes(VisConnectedShapesFlags Flags, string CategoryFilter)
        {
            throw new NotImplementedException();
        }

        public Array GluedShapes(VisGluedShapesFlags Flags, string CategoryFilter, Shape pOtherConnectedShape)
        {
            throw new NotImplementedException();
        }

        public void Disconnect(VisConnectorEnds ConnectorEnd, double OffsetX, double OffsetY, VisUnitCodes Units)
        {
            throw new NotImplementedException();
        }

        public void Resize(VisResizeDirection Direction, double Distance, VisUnitCodes UnitCode)
        {
            throw new NotImplementedException();
        }

        public void AddToContainers()
        {
            throw new NotImplementedException();
        }

        public void RemoveFromContainers()
        {
            throw new NotImplementedException();
        }

        public Page CreateSubProcess()
        {
            throw new NotImplementedException();
        }

        public Selection MoveToSubprocess(Page Page, object ObjectToDrop, out Shape NewShape)
        {
            throw new NotImplementedException();
        }

        public void DeleteEx(int DelFlags)
        {
            throw new NotImplementedException();
        }

        public Shape ReplaceShape(object MasterOrMasterShortcutToDrop, int ReplaceFlags = 0)
        {
            throw new NotImplementedException();
        }

        public void SetQuickStyle(VisQuickStyleMatrixIndices lineMatrix, VisQuickStyleMatrixIndices fillMatrix, VisQuickStyleMatrixIndices effectsMatrix, VisQuickStyleMatrixIndices fontMatrix, VisQuickStyleColors lineColor, VisQuickStyleColors fillColor, VisQuickStyleColors shadowColor, VisQuickStyleColors fontColor)
        {
            throw new NotImplementedException();
        }

        public double ChangePicture(string FileName, int ChangePictureFlags = 0)
        {
            throw new NotImplementedException();
        }

        public void VisualBoundingBox(short Flags, out double lpr8Left, out double lpr8Bottom, out double lpr8Right, out double lpr8Top)
        {
            throw new NotImplementedException();
        }

        public void VisualizeData(int DataRecordsetID)
        {
            throw new NotImplementedException();
        }

        public Document Document => throw new NotImplementedException();

        public object Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Application Application => throw new NotImplementedException();

        public short Stat => throw new NotImplementedException();

        public Master Master => throw new NotImplementedException();

        public short Type => throw new NotImplementedException();

        public short ObjectType => throw new NotImplementedException();

        public Cell get_Cells(string localeSpecificCellName)
        {
            throw new NotImplementedException();
        }

        public Cell get_CellsSRC(short Section, short Row, short Column)
        {
            throw new NotImplementedException();
        }

        public Shapes Shapes => throw new NotImplementedException();

        public string Data1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Data2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Data3 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Help { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string NameID => throw new NotImplementedException();

        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Text { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int CharCount => throw new NotImplementedException();

        public Characters Characters => throw new NotImplementedException();

        public short OneD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public short GeometryCount => throw new NotImplementedException();

        public short get_RowCount(short Section)
        {
            throw new NotImplementedException();
        }

        public short get_RowsCellCount(short Section, short Row)
        {
            throw new NotImplementedException();
        }

        public short get_RowType(short Section, short Row)
        {
            throw new NotImplementedException();
        }

        public void set_RowType(short Section, short Row, short lpi2Ret)
        {
            throw new NotImplementedException();
        }

        public Connects Connects => throw new NotImplementedException();

        public short Index16 => throw new NotImplementedException();

        public string Style { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string StyleKeepFmt { set => throw new NotImplementedException(); }
        public string LineStyle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string LineStyleKeepFmt { set => throw new NotImplementedException(); }
        public string FillStyle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string FillStyleKeepFmt { set => throw new NotImplementedException(); }
        public string TextStyle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string TextStyleKeepFmt { set => throw new NotImplementedException(); }

        public double old_AreaIU => throw new NotImplementedException();

        public double old_LengthIU => throw new NotImplementedException();

        public object get_GeomExIf(short fFill, double LineRes)
        {
            throw new NotImplementedException();
        }

        public string get_UniqueID(short fUniqueID)
        {
            throw new NotImplementedException();
        }

        public Page ContainingPage => throw new NotImplementedException();

        public Master ContainingMaster => throw new NotImplementedException();

        public Shape ContainingShape => throw new NotImplementedException();

        public short get_SectionExists(short Section, short fExistsLocally)
        {
            throw new NotImplementedException();
        }

        public short get_RowExists(short Section, short Row, short fExistsLocally)
        {
            throw new NotImplementedException();
        }

        public short get_CellExists(string localeSpecificCellName, short fExistsLocally)
        {
            throw new NotImplementedException();
        }

        public short get_CellsSRCExists(short Section, short Row, short Column, short fExistsLocally)
        {
            throw new NotImplementedException();
        }

        public short LayerCount => throw new NotImplementedException();

        public Layer get_Layer(short Index)
        {
            throw new NotImplementedException();
        }

        public EventList EventList => throw new NotImplementedException();

        public short PersistsEvents => throw new NotImplementedException();

        public string ClassID => throw new NotImplementedException();

        public short ForeignType => throw new NotImplementedException();

        public object Object => throw new NotImplementedException();

        public short ID16 => throw new NotImplementedException();

        public Connects FromConnects => throw new NotImplementedException();

        public Hyperlink Hyperlink => throw new NotImplementedException();

        public string ProgID => throw new NotImplementedException();

        public short ObjectIsInherited => throw new NotImplementedException();

        public Paths Paths => throw new NotImplementedException();

        public Paths PathsLocal => throw new NotImplementedException();

        public int ID => throw new NotImplementedException();

        public int Index => throw new NotImplementedException();

        public Section get_Section(short Index)
        {
            throw new NotImplementedException();
        }

        public Hyperlinks Hyperlinks => throw new NotImplementedException();

        public short get_SpatialRelation(Shape OtherShape, double Tolerance, short Flags)
        {
            throw new NotImplementedException();
        }

        public double get_DistanceFrom(Shape OtherShape, short Flags)
        {
            throw new NotImplementedException();
        }

        public double get_DistanceFromPoint(double x, double y, short Flags, out object pvPathIndex, out object pvCurveIndex, out object pvt)
        {
            throw new NotImplementedException();
        }

        public Selection get_SpatialNeighbors(short Relation, double Tolerance, short Flags, object ResultRoot)
        {
            throw new NotImplementedException();
        }

        public Selection get_SpatialSearch(double x, double y, short Relation, double Tolerance, short Flags)
        {
            throw new NotImplementedException();
        }

        public Cell get_CellsU(string localeIndependentCellName)
        {
            throw new NotImplementedException();
        }

        public string NameU { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public short get_CellExistsU(string localeIndependentCellName, short fExistsLocally)
        {
            throw new NotImplementedException();
        }

        public short get_CellsRowIndex(string localeSpecificCellName)
        {
            throw new NotImplementedException();
        }

        public short get_CellsRowIndexU(string localeIndependentCellName)
        {
            throw new NotImplementedException();
        }

        public bool IsOpenForTextEdit => throw new NotImplementedException();

        public Shape RootShape => throw new NotImplementedException();

        public Shape MasterShape => throw new NotImplementedException();

        public IPictureDisp Picture => throw new NotImplementedException();

        public Array ForeignData => throw new NotImplementedException();

        public int Language { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public double get_AreaIU(bool fIncludeSubShapes = false)
        {
            throw new NotImplementedException();
        }

        public double get_LengthIU(bool fIncludeSubShapes = false)
        {
            throw new NotImplementedException();
        }

        public int ContainingPageID => throw new NotImplementedException();

        public int ContainingMasterID => throw new NotImplementedException();

        public Master DataGraphic { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsDataGraphicCallout => throw new NotImplementedException();

        public ContainerProperties ContainerProperties => throw new NotImplementedException();

        public Array MemberOfContainers => throw new NotImplementedException();

        public bool IsCallout => throw new NotImplementedException();

        public Shape CalloutTarget { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Array CalloutsAssociated => throw new NotImplementedException();

        public Comments Comments => throw new NotImplementedException();

        public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AlternativeText { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int NavigationIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

#pragma warning disable CS0067
        public event EShape_CellChangedEventHandler CellChanged;
        public event EShape_ShapeAddedEventHandler ShapeAdded;
        public event EShape_BeforeSelectionDeleteEventHandler BeforeSelectionDelete;
        public event EShape_ShapeChangedEventHandler ShapeChanged;
        public event EShape_SelectionAddedEventHandler SelectionAdded;
        public event EShape_BeforeShapeDeleteEventHandler BeforeShapeDelete;
        public event EShape_TextChangedEventHandler TextChanged;
        public event EShape_FormulaChangedEventHandler FormulaChanged;
        public event EShape_ShapeParentChangedEventHandler ShapeParentChanged;
        public event EShape_BeforeShapeTextEditEventHandler BeforeShapeTextEdit;
        public event EShape_ShapeExitedTextEditEventHandler ShapeExitedTextEdit;
        public event EShape_QueryCancelSelectionDeleteEventHandler QueryCancelSelectionDelete;
        public event EShape_SelectionDeleteCanceledEventHandler SelectionDeleteCanceled;
        public event EShape_QueryCancelUngroupEventHandler QueryCancelUngroup;
        public event EShape_UngroupCanceledEventHandler UngroupCanceled;
        public event EShape_QueryCancelConvertToGroupEventHandler QueryCancelConvertToGroup;
        public event EShape_ConvertToGroupCanceledEventHandler ConvertToGroupCanceled;
        public event EShape_QueryCancelGroupEventHandler QueryCancelGroup;
        public event EShape_GroupCanceledEventHandler GroupCanceled;
        public event EShape_ShapeDataGraphicChangedEventHandler ShapeDataGraphicChanged;
        public event EShape_ShapeLinkAddedEventHandler ShapeLinkAdded;
        public event EShape_ShapeLinkDeletedEventHandler ShapeLinkDeleted;
#pragma warning restore
    }
}
