import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Grid } from "@material-ui/core";

import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import "../../pages/Pages.css";
import "../slider/HomeSlider";
import "bootstrap/dist/css/bootstrap.css";
import MyServices from "./MyServices";
import Footer from "../Footer/Footer";
const useStyle = makeStyles((theme) => ({
  grid: {
    width: "100%",
    margin: "15px",
    padding: "1px",
    row: "20px",
    background: "#fff",
  },
  main: {
    width: "100%",
    background: "#fff",
    padding: "10px",
  },
  paper: {
    textAlign: "left",
    color: "#198754",
    background: "#fff",
    padding: "10px",
  },
  counter: {
    background: "#fff",
    color: "#fff",
    textAlign: "center",
    fontWeight: "900",
  },
  appBar: {
    color: "#000",
    background: "#fff",
  },
  margin: {
    marginLeft: "100px",
    marginTop: "5px",
    color: "primary",
  },
  breadcrum: {
    backgroundColor: "#fff",
    padding: "10px",
    marginLeft: "100px",
  },
}));

function App() {
  const classes = useStyle();
  const el = document.querySelector(".counter");
  return (
    <div class="mmenu">
      <Grid container spacing={2} className="grid">
        <MyServices />
        <Grid item xs={1} md={1}></Grid>
  
      </Grid>
    </div>
  );
}

export default App;
