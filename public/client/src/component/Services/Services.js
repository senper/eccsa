import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Nav, Tab, Row, Col } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.css";
import { Breadcrumbs, Grid, Paper } from "@material-ui/core";
import {
  Accordion,
  AccordionSummary,
  AccordionDetails,
} from "@material-ui/core";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faLandmark } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import "./Services.css";
import {
  getServiceTypes,
  getServiceOwners,
  CertificateOfOriginId,
} from "../../services/servicetype";
import { useLocation } from "react-router-dom";
import Footer from "../Footer/Footer";
const useStyle = makeStyles((theme) => ({
  grid: {
    width: "100%",
    margin: "0px",
    padding: "3px",
  },
  breadcrumb: {
    padding: "10px",
    marginLeft: "100px",
  },
}));

export default function Services() {
  window.scrollTo({
    top: 0,
    behavior: "smooth",
  });
  const location = useLocation();
  const classes = useStyle();
  let serviceOwnerIdValue = 0;
  const [selectedServiceTypeId, setSelectedServiceTypeId] =
    React.useState(null);
  if (location.state) {
    const { serviceOwnerId } = location.state;
    serviceOwnerIdValue = serviceOwnerId;
  }
  React.useEffect(() => {
    if (location.state && location.state.serviceTypeId) {
      const { serviceTypeId } = location.state;
      setSelectedServiceTypeId(serviceTypeId);
    }
  }, [location.state]);
  const [serviceTypes, setServiceTypes] = React.useState([]);
  React.useEffect(() => {
    getServiceTypes().then((data) => {
      setServiceTypes(data.data.$values);
    });
  }, []);
  const [serviceOwners, setServiceOwners] = React.useState([]);
  React.useEffect(() => {
    getServiceOwners().then((data) => {
      setServiceOwners(data.data.$values);
    });
  }, [serviceOwnerIdValue]);

  function compare(a, b) {
    if (a.orderNo < b.orderNo) {
      return -1;
    }
    if (a.orderNo > b.orderNo) {
      return 1;
    }
    return 0;
  }

  function compare2(a, b) {
    if (serviceOwnerIdValue === a.idInt) {
      return -1;
    }
    if (serviceOwnerIdValue === b.idInt) {
      return 1;
    }
    return 0;
  }

  function compare3(a, b) {
    if (!selectedServiceTypeId) {
      return 0;
    }
    if (selectedServiceTypeId === a.id) {
      return -1;
    }
    if (selectedServiceTypeId === b.id) {
      return 1;
    }
    return 0;
  }

  function ToLocalStr(value) {
    return parseInt(value).toLocaleString();
  }

  return (
    <div>
      <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrumb}>
        <Link color="inherit" to="/">
          Home
        </Link>
        <Link
          as={Link}
          to={{
            pathname: "/services",
            state: {
              eventKey: "Services",
            },
          }}
        >
          Services
        </Link>
      </Breadcrumbs>
      <Grid container spacing={1}>
        {serviceOwners
          ? serviceOwners.sort(compare2).map((so) => (
              <>
                <Grid item xs={1} key={`1${so.$id}`}></Grid>
                <Grid item xs={10} key={`2${so.$id}`}>
                  <Accordion
                    id={`accordian${so.$id}`}
                    defaultExpanded={so.idInt === serviceOwnerIdValue}
                  >
                    <AccordionSummary
                      expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
                      style={{ marginBottom: "0" }}
                    >
                      <div class="icon">
                        {/* <FontAwesomeIcon icon={faLandmark} /> */}
                      </div>

                      <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                        <p class="links" style={{ marginBottom: "0" }}>
                          {" "}
                          {so.title} Services
                        </p>
                      </div>
                    </AccordionSummary>
                    <AccordionDetails
                      style={{ display: "block", marginTop: "-15px" }}
                    >
                      <Row style={{ paddingTop: "0px" }}>
                        <Col sm={12}>
                          <p>
                            <pre>{so.description}</pre>
                          </p>
                          {serviceTypes &&
                          serviceTypes.filter((s) => s.serviceOwner === so.id)
                            .length > 0 ? (
                            <Tab.Container
                              id="left-tabs-example"
                              defaultActiveKey={
                                serviceTypes
                                  .sort(compare3)
                                  .filter((s) => s.serviceOwner === so.id)[0]
                                  ? serviceTypes
                                      .sort(compare3)
                                      .filter(
                                        (s) => s.serviceOwner === so.id
                                      )[0].id
                                  : ""
                              }
                            >
                              <Row style={{ paddingTop: "10px" }}>
                                <Col sm={3}>
                                  <Nav
                                    variant="pills"
                                    style={{ display: "block" }}
                                  >
                                    {serviceTypes
                                      .sort(compare3)
                                      .filter((s) => s.serviceOwner === so.id)
                                      .map((s) => (
                                        <Nav.Item>
                                          <Nav.Link eventKey={s.id} key={s.$id}>
                                            {s.name}
                                          </Nav.Link>
                                        </Nav.Item>
                                      ))}
                                  </Nav>
                                  <br />
                                  <br />
                                  <br />
                                </Col>
                                <Col sm={9}>
                                  <Tab.Content>
                                    {serviceTypes
                                      ? serviceTypes.sort(compare3).map((s) => (
                                          <Tab.Pane
                                            key={`${so.$id}-${s.$id}`}
                                            eventKey={s.id}
                                          >
                                            <pre>{s.descriptionEng}</pre>{" "}
                                            {s.descriptionLocal ? (
                                              <>
                                                <br /> {s.descriptionLocal}{" "}
                                              </>
                                            ) : null}
                                            Service Charge:- Members:{" "}
                                            {ToLocalStr(s.memberServiceCharge)}{" "}
                                            ; Other:{" "}
                                            {ToLocalStr(
                                              s.nonMemberServiceCharge
                                            )}
                                            <br /> Requirements
                                            <ol>
                                              {s.serviceRequirements.$values
                                                .sort(compare)
                                                .map((r) => (
                                                  <li>
                                                    {r.name} -{" "}
                                                    {r.descriptionEng}{" "}
                                                  </li>
                                                ))}
                                            </ol>
                                            {s.id == CertificateOfOriginId ? (
                                              <>
                                                <a
                                                  class="btn btn-success"
                                                  href="https://esw.et/esw-trd/"
                                                >
                                                  Request Service
                                                </a>
                                              </>
                                            ) : (
                                              <Link
                                                class="btn btn-success"
                                                to={{
                                                  pathname:
                                                    "/serviceRequestForm",
                                                  state: {
                                                    serviceTypeId: s.id,
                                                  },
                                                }}
                                              >
                                                {" "}
                                                Request Service
                                              </Link>
                                            )}
                                          </Tab.Pane>
                                        ))
                                      : null}
                                  </Tab.Content>
                                </Col>
                              </Row>
                            </Tab.Container>
                          ) : null}
                        </Col>
                      </Row>
                    </AccordionDetails>
                    {/* <a href="/about" class="btn btn-success">Read More </a> */}
                  </Accordion>
                </Grid>
                <Grid item xs={1} key={`3${so.$id}`}></Grid>
              </>
            ))
          : null}
      </Grid>
    </div>
  );
}
