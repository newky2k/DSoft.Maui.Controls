using System.Data;

namespace MauiSampleApp;

public partial class DataGridPage : ContentPage
{
    public DataGridPage()
    {
        InitializeComponent();
        DataGrid.DataSource = BuildSampleTable();
    }

    private static DataTable BuildSampleTable()
    {
        var table = new DataTable("Employees");

        var idCol = table.Columns.Add("ID", typeof(int));
        idCol.ExtendedProperties["Width"] = 60.0;
        idCol.ExtendedProperties["AllowSort"] = true;

        var nameCol = table.Columns.Add("Name", typeof(string));
        nameCol.ExtendedProperties["AllowSort"] = true;

        var deptCol = table.Columns.Add("Department", typeof(string));
        deptCol.ExtendedProperties["AllowSort"] = true;

        var salaryCol = table.Columns.Add("Salary", typeof(string));
        salaryCol.ExtendedProperties["Width"] = 100.0;
        salaryCol.ExtendedProperties["AllowSort"] = false;

        var startedCol = table.Columns.Add("Started", typeof(string));
        startedCol.ExtendedProperties["Width"] = 90.0;
        startedCol.ExtendedProperties["AllowSort"] = true;

        object[][] rows =
        [
            [1,  "Alice Martin",   "Engineering",  "$95,000",  "Jan 2020"],
            [2,  "Bob Chen",       "Design",        "$82,000",  "Mar 2019"],
            [3,  "Carol Davis",    "Engineering",  "$105,000", "Jun 2018"],
            [4,  "Dan Patel",      "Marketing",    "$74,000",  "Sep 2021"],
            [5,  "Eva Russo",      "Engineering",  "$98,000",  "Feb 2022"],
            [6,  "Frank Kim",      "HR",            "$68,000",  "Nov 2017"],
            [7,  "Grace Lee",      "Design",        "$87,000",  "Apr 2020"],
            [8,  "Henry Torres",   "Marketing",    "$71,000",  "Aug 2023"],
            [9,  "Irene Nguyen",   "Engineering",  "$112,000", "Jan 2016"],
            [10, "James O'Brien",  "HR",            "$65,000",  "May 2022"],
            [11, "Karen Smith",    "Engineering",  "$99,000",  "Oct 2019"],
            [12, "Leo Fernandez",  "Design",        "$84,000",  "Jul 2021"],
        ];

        foreach (var row in rows)
            table.Rows.Add(row);

        return table;
    }

    private void OnRowSelected(object? sender, DSoft.Maui.Controls.DataGridRowSelectedEventArgs e)
    {
        var name    = e.Row["Name"];
        var dept    = e.Row["Department"];
        var salary  = e.Row["Salary"];
        var started = e.Row["Started"];

        SelectionLabel.Text = $"Row {e.RowIndex + 1}: {name} · {dept} · {salary} · Started {started}";
    }

    private async void OnCloseClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
