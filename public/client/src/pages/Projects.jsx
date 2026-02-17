import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import { Grid, Paper } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../App.css";
import "../component/slider/HomeSlider";
import { classExpression } from "@babel/types";
import HomeSlider from "../component/slider/HomeSlider";
import QuickLinks from "../component/NavMenus/QuickLinks";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../component/Footer/Footer";
import Breadcrumbs from "@material-ui/core/Breadcrumbs";
import { Link } from "react-router-dom";

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
                  <h5 class="section-title h3">Projects</h5>
                  <br />
                  <p>
                    {" "}
                    <strong>1.&nbsp; Phased Out</strong>
                  </p>
                  <ul>
                    <li>ECCSA-HWK Partnership Project</li>
                  </ul>
                  <p>&nbsp;</p>
                  <p>
                    <strong>2.&nbsp; Ongoing</strong>
                  </p>
                  <ul>
                    <li>
                      <a
                        href="http://ethiopianchamber.com/psd-hub.aspx"
                        target="_blank"
                      >
                        PSD-Hub
                      </a>{" "}
                      (SIDA)
                    </li>
                    <li>
                      <a
                        href="http://ethiopianchamber.com/public-and-private-consultative-forum-ppcf.aspx"
                        target="_blank"
                      >
                        EPPCF{" "}
                      </a>
                      (IFC)
                    </li>
                    <li>
                      <a
                        href="http://academy.ethiopianchamber.com/"
                        target="_blank"
                      >
                        Chamber Academy
                      </a>{" "}
                      (CIPE)
                    </li>
                    <li>
                      <a
                        href="http://ethiopianchamber.com/resource-center.aspx"
                        target="_blank"
                      >
                        Resource Center
                      </a>{" "}
                      (ITC)
                    </li>
                    <li>
                      <a href="http://ethiopianchamber.com/gem" target="_blank">
                        GEM{" "}
                      </a>
                      (EU)
                    </li>
                  </ul>
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
