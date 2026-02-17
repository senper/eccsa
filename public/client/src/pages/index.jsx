import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import { Grid, Paper, Button, Hidden } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../App.css";
import "./Pages.css";
import "../component/slider/HomeSlider";
import HomeSlider from "../component/slider/HomeSlider";
import { Link } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.css";
//import Numbers from "../component/Numbers/Numbers";
import NewsHighlight from "../component/News/NewsHighlight";
import ServiceModules from "../component/Services/ServiceModules";
import Partners2 from "../component/Teams/Partners";
import Footer from "../component/Footer/Footer";
import Faq from "../component/Faq/Faq";
import EventsSection from "../component/Event/EventsSection";
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
}));

function App() {
  const classes = useStyle();
  const el = document.querySelector(".counter");
  window.scrollTo({
    top: 0,
    behavior: "smooth",
  });
  return (
    <div class="mmenu">
      <div className={classes.main}>
        <Grid
          container
          spacing={2}
          className="grid"
          style={{ paddingTop: "0" }}
        >
          {/*  <Grid elevation={0} item xs={12} md={12} style={{padding: "0"}}>
            <FlashNews />
          </Grid> */}
          <Grid item xs={12} md={12}>
            <Paper elevation={3} variant="outlined" square>
              <HomeSlider />
            </Paper>
          </Grid>
          {/* <Grid elevation={0} item xs={12} md={12}>
            <Numbers />
          </Grid> */}
          <Grid item xs={12} md={12} className={classes.counter}>
            <div class="row">
              <div class="bg">Our Services</div>
            </div>
            <Grid container spacing={5}>
              <Grid item sm={2}>
                {/* <Hidden xsDown>
                  <span style={{ color: "black" }}>Advertising place</span>
                </Hidden> */}
              </Grid>
              <Grid item xs={12} sm={7}>
                <ServiceModules />
              </Grid>
              <Grid item sm={2}>
                {/* <span style={{ color: "black" }}>Advertising place 2</span> */}
              </Grid>
            </Grid>
          </Grid>
          <Grid item xs={12} md={12}>
            <div class="row">
              <div class="bg">Recent News</div>
            </div>
            <NewsHighlight />
          </Grid>
          <EventsSection />
          <Grid item xs={12} md={12}></Grid>
          <Grid item xs={12} md={12}>
            
            <Faq />
            {/* <Link
              eventKey="membership1"
              to="/membership1"
              class="btn btn-success"
            >
              {" "}
              Become a Member
            </Link> */}
          </Grid>
          {/* <Grid item xs={12} md={6}>
            <div class="row">
              <div class="bg">Ads</div>
            </div>
            <Ads /> 
          </Grid>*/}
          <Grid item xs={12} md={12}>
            <div class="row">
              <div class="bg">Our Partners</div>
            </div>
            <Partners2 />
          </Grid>
        </Grid>
      </div>
    </div>
  );
}

export default App;
