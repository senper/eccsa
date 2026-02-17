import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import { Grid } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import "../../pages/Pages.css";
import "../slider/HomeSlider";
import "bootstrap/dist/css/bootstrap.css";
import { Link } from "react-router-dom";
import Breadcrumbs from "@material-ui/core/Breadcrumbs";
import Footer from "../Footer/Footer";
import CertificateOfOriginDetail from "./CertificateOfOriginDetail";
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
    // padding:theme.spacing(1),
    textAlign: "left",
    color: "#198754",
    background: "#fff",
    padding: "10px",

    // elevation:'0',
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

 export default function CertificateOfOriginDetailPage() {
  const classes = useStyle();
  return (
    <div class="mmenu">
      <Grid container spacing={2} className="grid">
        <Grid item xs={12} md={12}>
          <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
            <Link to={{
                  pathname: "/",
                  state: {
                    eventKey: "default",
                  },
                }}>Home</Link>
            <Link to={{
              pathname: "/myServices",
              state: {
                navTo: "request",
              },
            }}>Requested Services</Link>
            <a href="javascript:void(0)">Certificate Of Origin Detail</a>
          </Breadcrumbs>
        </Grid>
        <Grid item xs={1} md={1}>
          {/* <Welcome /> */}
        </Grid>
        <Grid item xs={10} md={10}>
          <CertificateOfOriginDetail />
        </Grid>
        <Grid item xs={1} md={1}></Grid>
  
      </Grid>
    </div>
  );
}

