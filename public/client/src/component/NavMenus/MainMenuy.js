import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { Navbar, Nav, NavDropdown, Container, Carousel } from "react-bootstrap";
import "./MainMenu.css";
import "../../App.css";
import { makeStyles } from "@material-ui/core/styles";
import { createTheme } from "@mui/material/styles";
import "bootstrap/dist/css/bootstrap.min.css";
import { LoginMenu } from "../login/LoginMenu";
import { useLocation } from "react-router-dom";
import { useSelector } from "react-redux";
import store from "../../store";
import { loadUserFromStorage } from "../../services/oauth";
import { Grid } from "@material-ui/core";
import { getAdImages } from "../../services/advert";
import { getServiceOwners } from "../../services/servicetype";
import { lawCategories } from "../../services/laws";
import { textMediaTypes } from "../../services/otherMedia";
import MenuSearchForm from "./MenuSearchForm";

const useStyles = makeStyles((theme) => ({
  main: {
    width: "100%",
    background: "#B6E6F4",
    fontWeight: "600",
    fontStyle: "normal",

    row: "10px",
    //   textAlign:'left',
    color: "#ffffff",
    alignContent: "center",
    padding: "10px",
    opacity: "2.42",
    // elevation:'0',
  },

  margin: {
    marginLeft: "200px",
    marginTop: "5px",
    marginBottom: "0px",
    // color:'primary',
    // float: 'right',
  },
  fixedTop: {
    position: "fixed",
    top: "0",
    right: "0",
    left: "0",
    zIndex: "1030",
  },
  fixedBottom: {
    position: "fixed",
    top: "50",
    right: "5",
    left: "0",
    zIndex: "1040",
  },
}));

const advertHereList = ["bottomad1.gif", "bottomad2.gif"];
export default function MainMenu({ setIsLoggedIn }) {
  const classes = useStyles();
  const [active, setActive] = useState("default");
  const location = useLocation();

  var token = localStorage.getItem("access_token");
  const [user, setUser] = useState(useSelector((state) => state.auth.user));
  useEffect(() => {
    if ((!user || !user.id) && token) {
      loadUserFromStorage(store).then((userInput) => {
        token = localStorage.getItem("access_token");
        setUser(userInput ? userInput.data : null);
        setIsLoggedIn(true);
      });
    } else if (!token) {
      setUser({});
      setIsLoggedIn(false);
    }
  }, [token]);

  const [adImages, setAdImages] = useState([]);
  useEffect(() => {
    getAdImages(1).then((data) => {
      setAdImages(data.data);
    });
  }, []);

  const [serviceOwners, setServiceOwners] = useState([]);
  const [servicesEventKey, setServicesEventKey] = useState([]);
  useEffect(() => {
    getServiceOwners().then((data) => {
      setServiceOwners(data.data.$values);
    });
  }, []);
  useEffect(() => {
    if (location.state) {
      const { eventKey } = location.state;
      if (
        eventKey &&
        serviceOwners &&
        serviceOwners.find((s) => s.title == eventKey)
      ) {
        setServicesEventKey(
          serviceOwners.find((s) => s.title == eventKey).title
        );
      }
      if (eventKey) {
        setActive(eventKey);
      }
    }
  }, [location]);

  //const [position, setPosition] = useState(window.pageYOffset);
  const [visible, setVisible] = useState(true);
  // useEffect(() => {
  //   const handleScroll = () => {
  //     let moving = window.pageYOffset;
  //     console.log(window.pageYOffset);

  //     setVisible(0 >= moving);//setVisible(position > moving);
  //     //setPosition(moving);
  //   };
  //   window.addEventListener("scroll", handleScroll);
  //   return () => {
  //     window.removeEventListener("scroll", handleScroll);
  //   };
  // });

  const cls = visible ? "visible" : "hidden";
  return (
    <div className={`${classes.fixedTop} mmenu `}>
      {/* <Topbar/> */}
      <Grid container spacing={1} style={{display: visible ? "" : "none"}}>
        <Grid item sm={5}>
          <Link
            as={Link}
            to={{
              pathname: "/",
              state: {
                eventKey: "default",
              },
            }}
          >
            <img src="/images/header.jpg" href="/" width={"100%"}></img>
          </Link>
        </Grid>
        <Grid item sm={5}>
          <Carousel
            variant="light"
            indicators={false}
            style={{ paddingTop: "20px" }}
          >
            {adImages && adImages.$values && adImages.$values.length > 0
              ? adImages.$values.map((img) => (
                  <Carousel.Item>
                    <img
                      src={
                        process.env.REACT_APP_API_URL +
                        "/AdvertisingImages/header/" +
                        img
                      }
                      style={{
                        width: "100%",
                        height: "100%",
                        maxHeight: "78px",
                      }}
                    ></img>
                  </Carousel.Item>
                ))
              : advertHereList.map((img) => (
                  <Carousel.Item>
                    <img
                      src={"/AdvertisingImages/" + img}
                      style={{ width: "100%", height: "100%" }}
                    ></img>
                  </Carousel.Item>
                ))}
          </Carousel>
        </Grid>
        <Grid item sm={2}>
          <LoginMenu />
        </Grid>
      </Grid>
      <Navbar
        collapseOnSelect
        className={classes.main}
        expand="lg"
        class="dark2"
      >
        <Container>
          <Navbar.Brand href="/"></Navbar.Brand>
          <Navbar.Toggle aria-controls="responsive-navbar-nav" />
          <Navbar.Collapse id="responsive-navbar-nav">
            <Nav
              className="me-auto"
              activeKey={active}
              onSelect={(selectedKey) => setActive(selectedKey)}
            >
              <Nav.Link as={Link} to="/" eventKey="default">
                Home
              </Nav.Link>
              <NavDropdown
                title="About Us"
                href="/about"
                id="basic-nav-dropdown"
                eventKey="about"
              >
                <NavDropdown.Item as={Link} eventKey="about" to="/about">
                  About the Chamber
                </NavDropdown.Item>
                <NavDropdown.Item as={Link} eventKey="assembly" to="/assembly">
                  General Assembly{" "}
                </NavDropdown.Item>
                <NavDropdown.Item as={Link} to="/board" eventKey="board">
                  Board of Directors
                </NavDropdown.Item>
                <NavDropdown.Item as={Link} eventKey="team" to="/team">
                  {" "}
                  The Secretariat
                </NavDropdown.Item>
                <NavDropdown.Item as={Link} eventKey="former" to="/former">
                  Former Presidents & Secretary G.
                </NavDropdown.Item>
                <NavDropdown.Item as={Link} eventKey="location" to="/location">
                  Our Location
                </NavDropdown.Item>
                <NavDropdown.Item as={Link} eventKey="partners" to="/partners">
                  Our Partners
                </NavDropdown.Item>
                <NavDropdown.Item as={Link} eventKey="splan" to="/splan">
                  Strategic Plan
                </NavDropdown.Item>
                <NavDropdown.Item
                  as={Link}
                  eventKey="chamberlaw"
                  to="/chamberlaw"
                >
                  Chamber Law (+Directives)
                </NavDropdown.Item>
                <NavDropdown.Item as={Link} eventKey="projects" to="/projects">
                  ECCSA Projects
                </NavDropdown.Item>
              </NavDropdown>
              <NavDropdown
                title="Services"
                href="/services"
                id="basic-nav-dropdown"
                eventKey={servicesEventKey}
              >
                {serviceOwners
                  ? serviceOwners.map((so) => (
                      <>
                        <NavDropdown.Item
                          as={Link}
                          eventKey={so.title}
                          to={{
                            pathname: "/services",
                            state: {
                              serviceOwnerId: so.idInt,
                              eventKey: so.title,
                            },
                          }}
                        >
                          {so.title}
                        </NavDropdown.Item>
                      </>
                    ))
                  : null}
                <NavDropdown.Item
                  as={Link}
                  eventKey="MatchMaking"
                  to={{
                    pathname: "/matchMaking",
                    state: {
                      eventKey: "MatchMaking",
                    },
                  }}
                >
                  Match Making
                </NavDropdown.Item>
                <NavDropdown.Item
                  as={Link}
                  eventKey="VerifyCertificate"
                  to={{
                    pathname: "/verifyCertificate/ ",
                    state: {
                      eventKey: "VerifyCertificate",
                    },
                  }}
                >
                  Verify Certificate
                </NavDropdown.Item>
              </NavDropdown>

              <NavDropdown
                title="Membership"
                href="/membership"
                id="basic-nav-dropdown"
                eventKey="membership"
              >
                <NavDropdown.Item as={Link} eventKey="membership" to="/membership">
                  Our Members
                </NavDropdown.Item>
                <NavDropdown.Item as={Link} eventKey="bmo" to="/bmo">
                  Business Membership Organizations
                </NavDropdown.Item>
                
              </NavDropdown>
              <Nav.Link
                as={Link}
                to="/events"
                class="navitems"
                eventKey="events"
              >
                Events
              </Nav.Link>
              <NavDropdown
                title=" Media "
                href="#"
                id="basic-nav-dropdown"
                eventKey="#"
              >
                <NavDropdown.Item as={Link} eventKey="news" to="/news">
                  News{" "}
                </NavDropdown.Item>
                {textMediaTypes.map((so) => (
                  <>
                    <NavDropdown.Item
                      as={Link}
                      eventKey={so.text}
                      to={{
                        pathname: "/textMedias",
                        state: {
                          mediaCategory: so.value,
                          eventKey: so.text,
                        },
                      }}
                    >
                      {so.text}
                    </NavDropdown.Item>
                  </>
                ))}
                <NavDropdown.Item as={Link} eventKey="picture" to="/picture">
                  Photo Gallery{" "}
                </NavDropdown.Item>
                <NavDropdown.Item as={Link} eventKey="video" to="/video">
                  Video Gallery{" "}
                </NavDropdown.Item>
              </NavDropdown>
              <NavDropdown
                title="Resources "
                href="#"
                id="basic-nav-dropdown"
                eventKey="#"
              >
                <NavDropdown.Item
                  as={Link}
                  class="navitems"
                  to={{
                    pathname: "/eresources",
                    state: {
                      typeId: 0,
                      typeText: "e-Resources",
                    },
                  }}
                  eventKey="eresources"
                >
                  Resources{" "}
                </NavDropdown.Item>
                <NavDropdown.Item
                  as={Link}
                  class="navitems"
                  to="/businessDirectory"
                  eventKey="businessDirectory"
                >
                  Directory{" "}
                </NavDropdown.Item>
                <NavDropdown.Item
                  as={Link}
                  class="navitems"
                  to="/ppDialog"
                  eventKey="ppDialog"
                >
                  PPD{" "}
                </NavDropdown.Item>
                <NavDropdown.Item
                  as={Link}
                  class="navitems"
                  to="/businessInformation"
                  eventKey="businessInformation"
                >
                  Business Info.
                </NavDropdown.Item>
              </NavDropdown>

              <NavDropdown
                title=" Profile "
                href="#"
                id="basic-nav-dropdown"
                eventKey="#"
              >
                <NavDropdown.Item
                  as={Link}
                  class="navitems"
                  eventKey="investmentProfileEt"
                  to={{
                    pathname: "/investmentProfile",
                    state: {
                      typeId: 3,
                    },
                  }}
                >
                  Ethiopia{" "}
                </NavDropdown.Item>
                <NavDropdown.Item
                  as={Link}
                  class="navitems"
                  eventKey="investmentProfileRegion"
                  to={{
                    pathname: "/investmentProfile",
                    state: {
                      typeId: 1,
                    },
                  }}
                >
                  Regions{" "}
                </NavDropdown.Item>
                <NavDropdown.Item
                  as={Link}
                  class="navitems"
                  eventKey="investmentProfileSector"
                  to={{
                    pathname: "/investmentProfile",
                    state: {
                      typeId: 2,
                    },
                  }}
                >
                  Sectors{" "}
                </NavDropdown.Item>
              </NavDropdown>
              <NavDropdown
                title=" Laws"
                href="#"
                id="basic-nav-dropdown"
                eventKey="#"
              >
                {lawCategories.map((so) => (
                  <>
                    <NavDropdown.Item
                      as={Link}
                      eventKey={so.text}
                      to={{
                        pathname: "/laws",
                        state: {
                          lawCategory: so.value,
                          eventKey: so.text,
                        },
                      }}
                    >
                      {so.text} Laws
                    </NavDropdown.Item>
                  </>
                ))}
              </NavDropdown>
              <NavDropdown
                title=" Contact Us "
                href="#"
                id="basic-nav-dropdown"
                eventKey="#"
              >
                <NavDropdown.Item
                  as={Link}
                  class="navitems"
                  to="/issueForm"
                  eventKey="issueForm"
                >
                  NTB Form
                </NavDropdown.Item>
                <NavDropdown.Item
                  as={Link}
                  class="navitems"
                  to="/contact"
                  eventKey="contact"
                >
                  Contact Us{" "}
                </NavDropdown.Item>
              </NavDropdown>

              {token ? (
                <Nav.Link as={Link} to="/myServices" eventKey="myServices">
                  My Services
                </Nav.Link>
              ) : null}
            </Nav>
            <div className="searchform">
              <MenuSearchForm />
            </div>
          </Navbar.Collapse>
        </Container>
      </Navbar>
    </div>
  );
}

// export default MainMenu;
