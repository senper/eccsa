import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import { Grid, Paper } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../App.css";
import "../component/slider/HomeSlider";
import QuickLinks from "../component/NavMenus/QuickLinks";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../component/Footer/Footer";
import { Link } from "react-router-dom";
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
const secretaryGenerals =  [{
  photo: "sg.png",
  name: "Mr. Wubie Mengistu",
  position: "Secretary General",
  tele: "251-115548096",
  fax: "251-115517699",
  email: "wubiem@ethiopianchamber.com",
}];
const managements = [
  //{name:'', position: "", tele: "", fax: "", email: ""},
 
  {
    photo: "male.png",
    name: "Mr. Million Feleke",
    position: "Director, Advocacy Directorate",
    tele: "251-922409532",
    fax: "251-115502673",
    email: "millionf@ethiopianchamber.com",
  },
  {
    photo: "male.jfif",
    name: "Mr. Goitom Aderajew",
    position: "Director, Trade and Investment Promotion Directorate",
    tele: "251-914762281/251115502673",
    fax: "251-115517699",
    email: "egoitom123@gmail.com",
  },
  {
    photo: "male.jfif",
    name: "Mr. Debebe Abebe",
    position: "Director, Communication and public relation Directorate",
    tele: "251-911754195",
    fax: "251-115517699",
    email: "debebe60@yahoo.com",
  },
  {
    photo: "male.jfif",
    name: "Mr. Adinew Bekele",
    position: "ECCSA President Advisor",
    tele: "251-911619508/251-115540317",
    fax: "251-115517699",
    email: "adinewbekele9@gmail.com",
  },
  {
    photo: "male.jfif",
    name: "Mr. Selam Setegn",
    position: "Director, Legal and Arbitration Center Directorate",
    tele: "251-911641727/251115549684",
    fax: "251-115517699",
    email: "aselamsetegn@yahoo.com",
  },
  {
    photo: "male.jfif",
    name: "Mr. Abebe Mengistu",
    position: "Director, Institutional Support Directorate",
    tele: "251-9114297801/251115311048",
    fax: "251-115517699",
    email: "abebemengistu722@gmail.com",
  },
];
function App() {
  const classes = useStyle();
  return (
    <div class="mmenu">
      <Grid container spacing={2} className="grid">
        <Grid item xs={12} md={12}>
          <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
            <Link to="/"> Home</Link>
            <Link eventKey="about" aria-current="page" to="#">
              {" "}
              The Secretariat
            </Link>
          </Breadcrumbs>
        </Grid>

        <Grid item xs={1} md={1}>
        </Grid>
        <Grid item xs={11} md={8}>
          <Paper>
            <div class="row">
              <section id="team" class="pb-5">
                <div class="container">
                  <h5 class="section-title h3">ECCSA’s The Secretariat</h5>
                  {secretaryGenerals.map((m) => (
                  <div class="row">
                   
                   <div class="col-xs-12 col-sm-6 col-md-4">
                       
                      </div>
                      <div class="col-xs-12 col-sm-6 col-md-4">
                        <div class="image-flip">
                          <div class="mainflip flip-0">
                            <div class="frontside">
                              <div class="card">
                                <div
                                  class="card-body text-center"
                                  style={{ height: "400px", overflowY: "auto" }}
                                >
                                  { <p>
                                    {m.photo ? (
                                      <img
                                        class=" img-fluid"
                                        src={"images/management/" + m.photo}
                                        width="200px"
                                        alt="card"
                                      />
                                    ) : null}
                                  </p> }
                                  <h4 class="card-title">{m.name}</h4>
                                  <h6>{m.position}</h6>
                                  <p class="card-text">
                                    {m.tele ? (
                                      <>
                                        Tele: {m.tele}
                                        <br />
                                      </>
                                    ) : null}
                                    {m.tele ? (
                                      <>
                                        Fax: {m.fax}
                                        <br />
                                      </>
                                    ) : null}
                                    {m.tele ? <>E-mail: {m.email}</> : null}
                                  </p>
                                  {/* <a href="https://www.fiverr.com/share/qb8D02" class="btn btn-primary btn-sm"><i class="fa fa-plus"></i></a> */}
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    
                  </div>
                  ))}

                  <div class="row">
                    {managements.map((m) => (
                      <div class="col-xs-12 col-sm-6 col-md-4">
                        <div class="image-flip">
                          <div class="mainflip flip-0">
                            <div class="frontside">
                              <div class="card">
                                <div
                                  class="card-body text-center"
                                  style={{ height: "265px", overflowY: "auto" }}
                                >
                                  {/* <p>
                                    {m.photo ? (
                                      <img
                                        class=" img-fluid"
                                        src={"images/management/" + m.photo}
                                        width="200px"
                                        alt="card image"
                                      />
                                    ) : null}
                                  </p> */}
                                  <h4 class="card-title">{m.name}</h4>
                                  <h6>{m.position}</h6>
                                  <p class="card-text">
                                    {m.tele ? (
                                      <>
                                        Tele: {m.tele}
                                        <br />
                                      </>
                                    ) : null}
                                    {m.tele ? (
                                      <>
                                        Fax: {m.fax}
                                        <br />
                                      </>
                                    ) : null}
                                    {m.tele ? <>E-mail: {m.email}</> : null}
                                  </p>
                                  {/* <a href="https://www.fiverr.com/share/qb8D02" class="btn btn-primary btn-sm"><i class="fa fa-plus"></i></a> */}
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    ))}
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
