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
      {/* <img            
               
               className="d-block w-70"
                class="img-fluid"
                src="../images/header.jpg" 
                href="/"></img>  <a href="/login" class="btn btn-outline-success">Login/Register</a>
   <MainMenu/> */}

      <Grid container spacing={2} className="grid">
        <Grid item xs={12} md={12}>
          {/* <MainMenu/> */}

          <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
            <Link to="/#">Home</Link>

            <Link to="#">General Assembly</Link>
          </Breadcrumbs>
        </Grid>
        <Grid item xs={1} md={1}>
          {/* <Welcome /> */}
        </Grid>
        <Grid item xs={11} md={8}>
          <Paper>
            <div class="row">
              <div class="col-sm-12">
                <div class="card">
                  <div class="card-body">
                    <h3 class="card-title"> General Assembly </h3>
                    <p class="card-text">
                      The General Assembly is composed of the representatives of
                      member chambers of commerce and sectorial associations of
                      ECCSA. The General Assembly has the following powers and
                      duties:-
                      <ol>
                        <li>
                          to approve annual budget and work program of the
                          Chamber;
                        </li>
                        <li>to elect Board members of the Chamber;</li>
                        <li>
                          to assign an Auditor for the audit of the books of
                          accounts and financial documents of the Chamber;
                        </li>
                        <li>
                          to examine and approve reports submitted to it from
                          the Secretary General and the Auditor;
                        </li>
                        <li>
                          to approve internal regulations of the Chamber, and
                          give decisions on other matters concerning the
                          Chamber;
                        </li>
                      </ol>
                      The General Assembly may delegate its powers and duties to
                      the Board, the President, the Vice President or the
                      Secretary as appropriate.
                    </p>
                    <br />
                    {/* <a href="#" class="btn btn-success">Read More </a> */}
                  </div>
                </div>
              </div>
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
