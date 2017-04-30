namespace SlExport
{
  public enum RowState
  {
    // The resource is a new item (did not exist in previous scan) 
    New = 0,
    // The resource seems not be be used by the <source> (remnant of a previous scan) and can be removed 
    Unused = 1,
    // will not appear in the XML as this seems to be the default which will be omitted)
    InUse = 2,
    // the resource has changed since the previous scan and needs to be reviewed by a translator 
    Changed = 3,
  }
}