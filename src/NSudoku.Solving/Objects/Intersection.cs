using System.Collections.Generic;
using System.Linq;

namespace NSudoku.Solving.Objects;

/// <summary>
///  Encapsulates information about the intersection between two regions.
/// </summary>
public class Intersection
{
    private ICollection<Cell> _commonCells;
    private ICollection<Cell> _region1Cells;
    private ICollection<Cell> _region2Cells;

    public Intersection(ICellConstraint region1, ICellConstraint region2)
    {
        Region1 = region1;
        Region2 = region2;
    }

    public ICellConstraint Region1 { get; }

    public ICellConstraint Region2 { get; }

    public ICollection<Cell> CommonCells {
        get {
            if (_commonCells == null) {
                _commonCells = Region1.Cells.Intersect(Region2.Cells).ToList();
            }

            return _commonCells;
        }
    }

    public ICollection<Cell> Region1Cells {
        get {
            if (_region1Cells == null) {
                _region1Cells = Region1.Cells.Except(Region2.Cells).ToList();
            }

            return _region1Cells;
        }
    }

    public ICollection<Cell> Region2Cells {
        get {
            if (_region2Cells == null) {
                _region2Cells = Region2.Cells.Except(Region1.Cells).ToList();
            }

            return _region2Cells;
        }
    }

    public bool AreSame => Region1 == Region2;

    public bool Intersect => !AreSame && CommonCells.Any();
}
