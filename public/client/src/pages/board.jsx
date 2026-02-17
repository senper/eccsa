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

const boardMembers = [
  {
    photoUrl: "AEng Melaku.png",
    tel: "0918774646",
    email: "melakuezezew@gmail.com",
    name: "Engineer Melaku Ezezew (President)",
  },
  {
    photoUrl: "BVice Pres.png",
    tel: "0914301197 or 0911760512",
    email: "asefa.gie03@yahoo.com",
    name: "Mr. Aseffa Gebre Sillasie (V. President)",
  },
  {
    photoUrl: "CTemnet Solomon.png",
    tel: "0918003094",
    email: "temnisol@gmail.com",
    name: "Mrs. Temniet Selomon Haileselassie (Member)",
  },
  {
    photoUrl: "Dr Aynlm.png",
    tel: "0911256488",
    email: "goldieconsult@gmail.com",
    name: "Dr. Aynalem Abayneh (Member)",
  },
  {
    photoUrl: "EAbebayehu.png",
    tel: "0912672967",
    email: "technologyconstructionplc@gmail.com",
    name: "Mr. Abebayehu Girma (Member)",
  },
  {
    photoUrl: "GAto Ashenafi.png",
    tel: "0911396312",
    email: "etoshone@gmail.com",
    name: "Mr. Ashenafi Feleke (Member)",
  },
  {
    photoUrl: "HTeshal.png",
    tel: "0914706969 or 0914753588",
    email: "fisahesyon@yahoo.com",
    name: "Mr. Teshale Fisaheyon (Member)",
  },
  {
    photoUrl: "IAto Tadesse.png",
    tel: "0911208915",
    email: "Tadu0911208915@gmail.com",
    name: "Mr. Taddesse Genna (Member)",
  },
  {
    photoUrl: "JDr Hassun Abdulahi.png",
    tel: "0946430519",
    email: "hassanbade@gmail.com",
    name: "Dr. Hassen Abdulahi Bade (Member)",
  },
  {
    photoUrl: "KEngd.png",
    tel: "0916851621",
    email: "engidabeyene1621@gmail.com",
    name: "Mr. Engida Beyene (Member)",
  },
];
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
            <Link to="/"> Home</Link>
            <Link eventKey="about" aria-current="page" to="/board">
              {" "}
              Board of Directors
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
                  <h5 class="section-title h3">Board of Directors</h5>
                  <div class="row">
                    <p class="card-text">
                      The General Assembly elects a board of directors
                      comprising of 11 persons that directs Ethiopian Chamber of
                      Commerce and Sectoral Association (ECCSA) every two years.
                      The Board has the following powers and duties:
                      <ol>
                        <li>
                          to ensure the implementation of the decisions of the
                          General Assembly;
                        </li>
                        <li>to employ and dismiss the Secretary General;</li>
                        <li>
                          to decide on the employment and dismissal of the heads
                          of departments and services of the Chamber, submitted
                          to it by the Secretary General;
                        </li>
                        <li>
                          to call through the president extraordinary meetings
                          of the General Assembly as it deems necessary or when
                          one third of the members of the General Assembly
                          demand;
                        </li>
                        <li>
                          to submit annual budget and work program of the
                          Chamber to the General Assembly;;
                        </li>
                        <li>
                          to decide on matters submitted to it by the Secretary
                          General;
                        </li>
                        <li>
                          to establish different committees to assist the Board
                          in carrying out its duties;
                        </li>
                        <li>to perform such other duties as are necessary</li>
                      </ol>
                      The General Assembly may delegate its powers and duties to
                      the Board, the President, the Vice President or the
                      Secretary as appropriate.
                    </p>
                    <h3>The Board of Directors</h3>

                    {boardMembers.map((b) => (
                      <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="image-flip">
                          <div class="mainflip flip-0">
                            <div class="frontside">
                              <div class="card">
                                <div class="card-body text-center">
                                  <p>
                                    <img
                                      class=" img-fluid"
                                      src={"/images/boardMembers/" + b.photoUrl}
                                      width="200px"
                                      alt="card image"
                                    />
                                  </p>
                                  <h4 class="card-title">{b.name}</h4>
                                  <p class="card-text">
                                    Tele: {b.tel} <br />
                                    E-mail: {b.email}
                                  </p>
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
