/* eslint-disable react/prop-types */
import React from "react";
// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
import {
  DataGrid,
  GridToolbarContainer,
  GridToolbarExport,
} from "@material-ui/data-grid";
import QuickSearchToolbar from "./QuickSearchToolbar";
//import { hexToRgb } from "assets/jss/material-dashboard-pro-react";
//import { blackColor } from "assets/jss/material-dashboard-pro-react";

const useDGStyles = makeStyles((theme) => ({
  root: {
    "& .MuiDataGrid-columnsContainer": {
      border: "0",
      borderRadius: "3px",
      backgroundColor: "lightgray",
      boxShadow:
        "0 1px 1px -2px rgba(" +
        "0,0,0" +
        ", 0.42), 0 3px 20px 0px rgba(" +
        "0,0,0" +
        ", 0.12), 0 8px 10px -5px rgba(" +
        "0,0,0" +
        ", 0.2)",
    },
    "& .MuiDataGrid-row.Mui-odd": {
      backgroundColor: "aliceblue",
    },
    "& .MuiDataGrid-columnHeader": {
      boxShadow:
        "0 5px 10px -6px rgba(" +
        "0,0,0" +
        ", 0.42), 0 3px 20px 0px rgba(" +
        "0,0,0" +
        ", 0.12), 0 8px 10px -5px rgba(" +
        "0,0,0" +
        ", 0.2)",
    },
    "& .MuiDataGrid-cell": {
      border: "0",
      borderRadius: "4px",
      boxShadow:
        "0 1px 1px -1px rgba(" +
        "0,0,0" +
        ", 0.42), 0 1px 2px 0px rgba(" +
        "0,0,0" +
        ", 0.12), 0 1px 2px -1px rgba(" +
        "0,0,0" +
        ", 0.2)",
    },
    "& .MuiDataGrid-main": {
      boxShadow:
        "0 1px 1px -2px rgba(" +
        "0,0,0" +
        ", 0.42), 0 3px 20px 0px rgba(" +
        "0,0,0" +
        ", 0.12), 0 8px 10px -5px rgba(" +
        "0,0,0" +
        ", 0.2)",
    },
    "& .MuiDataGrid-footerContainer": {
      boxShadow:
        "0 1px 1px -2px rgba(" +
        "0,0,0" +
        ", 0.42), 0 3px 20px 0px rgba(" +
        "0,0,0" +
        ", 0.12), 0 8px 10px -5px rgba(" +
       // hexToRgb(blackColor) +
        ", 0.2)",
    },
  },
}));

function CustomToolbar() {
  return (
    <GridToolbarContainer>
      <GridToolbarExport />
      <QuickSearchToolbar />
    </GridToolbarContainer>
  );
}

export default function CustomDataGrid({
  dataRows,
  columns,
  height = 700,
  setAutoHeight = true,
  width = "100%",
}) {
  const dgClasses = useDGStyles();
  const [searchText, setSearchText] = React.useState("");
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
  //
  const requestSearch = (searchValue) => {
    setSearchText(searchValue);
    const searchRegex = new RegExp(escapeRegExp(searchValue), "i");
    const filteredRows = dataRows.filter((row) => {
      return Object.keys(row).some((field) => {
        return searchRegex.test(row[field].toString());
      });
    });
    setRows(filteredRows);
  };
  //
  const [pageSize, setPageSize] = React.useState(10);
  //
  return (
    <div
      style={
        setAutoHeight ? { width: width } : { height: height, width: width }
      }
    >
      <DataGrid
        autoHeight={setAutoHeight}
        className={dgClasses.root}
        rows={rows}
        columns={columns}
        components={{
          Toolbar: QuickSearchToolbar,
        }}
        pageSize={pageSize}
        onPageSizeChange={(newPageSize) => setPageSize(newPageSize)}
        rowsPerPageOptions={[5, 10, 20, 25, 50, 100]}
        pagination
        componentsProps={{
          toolbar: {
            value: searchText,
            onChange: (event) => requestSearch(event.target.value),
            clearSearch: () => requestSearch(""),
          },
        }}
      />
    </div>
  );
}
