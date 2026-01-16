using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common.Querying;

/// <summary>
/// Merepresentasikan satu aturan filter per kolom.
/// </summary>
public class FilterModel
{
    /// <summary>
    /// Nama kolom/field yang akan difilter.
    /// Contoh: "ba_type"
    /// </summary>
    public string? Field { get; set; }

    /// <summary>
    /// Operator pembanding.
    /// Contoh: "contains", "equals", "startsWith", "endsWith", ">", "<=", "!="
    /// </summary>
    public FilterOperator Operator { get; set; }

    /// <summary>
    /// Nilai yang akan digunakan untuk memfilter.
    /// </summary>
    public string? Value { get; set; }
}