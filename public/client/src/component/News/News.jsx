import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import "bootstrap/dist/css/bootstrap.min.css";
import "../slider/HomeSlider";
import "bootstrap/dist/css/bootstrap.css";
import News from "./News";
import Footer from "../Footer/Footer";
import { Grid } from "@material-ui/core";

const useStyle = makeStyles((theme) => ({
  grid: {
    width: "100%",
    margin: "0px",
    padding: "3px",
  },
  paper: {
    padding: theme.spacing(2),
    textAlign: "center",
    color: theme.palette.text.secondary,
    background: theme.palette.success.light,
  },
  breadcrum: {
    backgroundColor: "#fff",
    padding: "10px",
    marginLeft: "100px",
  },
}));

function App() {
  const classes = useStyle();
  return (
    <div class="mmenu">
      <News />

    </div>
  );
}

export default App;
