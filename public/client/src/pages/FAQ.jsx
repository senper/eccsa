import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Grid, Paper } from "@material-ui/core";
import { Link } from "react-router-dom";
import Breadcrumbs from "@material-ui/core/Breadcrumbs";
import "bootstrap/dist/css/bootstrap.min.css";
import "../App.css";
import "../component/slider/HomeSlider";
import QuickLinks from "../component/NavMenus/QuickLinks";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../component/Footer/Footer";

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
export default function FAQ() {
  const classes = useStyle();
  window.scrollTo({
    top: 0,
    behavior: "smooth",
  });
  return (
    <div class="mmenu">
      <Grid container spacing={2} className="grid">
        <Grid item xs={12} md={12}>
          <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
            <Link to="/"> Home</Link>
            <Link eventKey="about" aria-current="page" to="#">
              {" "}
              FAQ
            </Link>
          </Breadcrumbs>
        </Grid>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={8}>
          <h5 className="section-title h3">UNDER CONSTRUCTION </h5>
        </Grid>
        <Grid item xs={12} md={3}>
        </Grid>
  
      </Grid>
    </div>
  );
}
