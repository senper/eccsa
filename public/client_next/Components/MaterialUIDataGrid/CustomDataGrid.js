import React from "react";
import {
  DataGrid,
  GridToolbar,
} from "@mui/x-data-grid";

export default function CustomDataGrid({
  dataRows,
  columns,
  height = 700,
  setAutoHeight = true,
  width = "100%",
}) {
  const [rows, setRows] = React.useState([]);
  React.useEffect(() => {
    setRows(dataRows);
  }, [dataRows]);
  //
  function escapeRegExp(input) {
    const source =
      typeof input === "string" || input instanceof String ? input : "";
    return source.replace(/[-[/\]{}()*+?.,\\^$|#\s]/g, "\\$&");
  }
  const [pageSize, setPageSize] = React.useState(10);
  //
  return (
    <div
      style={
        setAutoHeight ? { width: width } : { height: height, width: width }
      }
    >
      <DataGrid
        disableColumnFilter
        disableColumnSelector
        disableDensitySelector
        autoHeight={setAutoHeight}
        className="data_grid_root"
        rows={rows}
        columns={columns}
        components={{ Toolbar: GridToolbar }}
        pageSize={pageSize}
        onPageSizeChange={(newPageSize) => setPageSize(newPageSize)}
        rowsPerPageOptions={[5, 10, 20, 25, 50, 100]}
        pagination
        componentsProps={{
          toolbar: {
            showQuickFilter: true,
            quickFilterProps: { debounceMs: 500 },
          },
        }}
      />
    </div>
  );
}
