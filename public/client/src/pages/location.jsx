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
function handleClick(event) {
  event.preventDefault();
}
function App() {
  const classes = useStyle();
  return (
    <div class="mmenu">
      <Grid container spacing={2} className="grid">
        <Grid item xs={12} md={12}>
          {/* <MainMenu/> */}

          <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
            <Link color="inherit" to="/">
              Home
            </Link>

            <Link color="textPrimary" to="#" aria-current="page">
              Location
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
                  <h5 class="section-title h3">Our Location</h5>
                  <img
                    src="../images/location.jpg"
                    width="700px"
                    height="450px"
                  ></img>
                  <div class="row">
                    <div class="address">
                      <ul class="list-unstyled mb-0">
                        <p>
                          <i class="fas fa-home me-3"></i> Ethiopian Chamber of
                          Commerce & Sectoral Associations Building, 1st Floor,
                          Mexico Square
                        </p>
                        <p>
                          <i class="fas fa-home me-3"></i> Addis Ababa, Ethiopia
                        </p>
                        <p>
                          <i class="fas fa-phone me-3"></i>Phone: + 251 115 514
                          005
                        </p>
                        <p>
                          <i class="fas fa-phone me-3"></i>Fax: 251-11-5517699
                        </p>
                        <p>
                          <i class="fas fa-phone me-3"></i>P.O.Box: 517
                        </p>
                        <p>
                          <i class="fas fa-print me-3"></i>{" "}
                          Email:info@ethiopianchamber.com
                        </p>
                        <p>
                          <i class="fas fa-print me-3"></i> Website:
                          www.ethiopianchamber.et
                        </p>
                      </ul>
                    </div>
                  </div>
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
