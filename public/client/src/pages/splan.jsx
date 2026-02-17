import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import { Grid, Paper } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../App.css";
import "../component/slider/HomeSlider";
import { Link } from "react-router-dom";
import Breadcrumbs from "@material-ui/core/Breadcrumbs";
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

function App() {
  const classes = useStyle();
  return (
    <div class="mmenu">
      <Grid container spacing={2} className="grid">
        <Grid item xs={12} md={12}>
          <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
            <Link color="inherit" to="/">
              Home
            </Link>

            <Link color="textPrimary" to="#" aria-current="page">
              Strategic Plan
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
                  <h5 class="section-title h3">Strategic Plan</h5>
                  <br />
                  <a href="http://ethiopianchamber.com/Data/Sites/1/eccsa-five-year-(2014-15-%E2%80%93-2018-19)-strategic-plan.pdf">
                    {" "}
                    ECCSA FIVE YEARS Strategic Plan of ECCSA [20014/15 -
                    2018/19] (Read/Download)
                  </a>{" "}
                  <br />
                  <br />
                  <a href="http://ethiopianchamber.com/Data/Sites/1/downloadables/Strategic%20Plan%20of%20ECCSA%20[2009%20-%202014].pdf">
                    {" "}
                    ECCSA FIVE YEARS Strategic Plan of ECCSA [2009 - 2014]
                    (Read/Download)
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
