import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import { Grid, Paper } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../App.css";
import "../component/slider/HomeSlider";
import { Link } from "react-router-dom";
import QuickLinks from "../component/NavMenus/QuickLinks";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../component/Footer/Footer";
import Breadcrumbs from "@material-ui/core/Breadcrumbs";

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
      {/* <img

                className="d-block w-70"
                class="img-fluid"
                src="../images/header.jpg"
                href="/"></img>  <a href="/login" class="btn btn-outline-success">Login/Register</a>
             <MainMenu/> */}

      <Grid container spacing={2} className="grid">
        <Grid item xs={12} md={12}>
          <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
            <Link color="inherit" to="/">
              Home
            </Link>

            <Link color="textPrimary" to="#" aria-current="page">
              Chamber Law
            </Link>
          </Breadcrumbs>
        </Grid>

        <Grid item xs={1} md={1}>
          {/* <Welcome /> */}
        </Grid>
        <Grid item xs={11} md={8}>
          <Paper>
            <div class="row">
              <section id="team" class="pb-5">
                <div class="container">
                  <h5 class="section-title h3">Chamber Laws and Directives</h5>
                  <br />
                  <a href="http://ethiopianchamber.com/Data/Sites/1/downloadables/Directive-Chamber%20of%20commerce%20and%20sectorial%20Association.doc">
                    Chamber of commerce and sectorial association directive
                  </a>{" "}
                  <br />
                  <br />
                  <a href="http://ethiopianchamber.com/Data/Sites/1/downloadables/CHAMBERS%20OF%20COMMERCE%20AND%20SECTORAL%20ASSOCIATIONS%20ESTABLISHMENT.pdf">
                    {" "}
                    Proc No. 341-2003 Chamber of commerce and Sectoral
                    Association
                  </a>{" "}
                  <br /> <br />{" "}
                </div>
              </section>
            </div>
          </Paper>
        </Grid>
        <Grid item xs={12} md={3}>
          <QuickLinks />
        </Grid>
      </Grid>
    </div>
  );
}

export default App;
