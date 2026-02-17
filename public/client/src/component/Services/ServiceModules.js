import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import {
  Accordion,
  AccordionSummary,
  AccordionDetails,
} from "@material-ui/core";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import {
  FontAwesomeIcon,
  FaBalanceScale,
} from "@fortawesome/react-fontawesome";
import {
  faBalanceScale,
  faHammer,
  faBullhorn,
} from "@fortawesome/free-solid-svg-icons";
import {
  Link,
} from "react-router-dom";
import "./Services.css";
import { getServiceOwners } from "../../services/servicetype";

export default function ServiceModules() {  
  const [serviceOwners, setServiceOwners] = React.useState([]);
  React.useEffect(() => {
    getServiceOwners().then((data) => {
      setServiceOwners(data.data.$values);
    });
  }, []);

  return (
    <div>
      <hr />

      {/* <Grid container spacing={5}> */}
        {serviceOwners ? serviceOwners.map((so) => (
          <>
           {/*  <Grid item xs={2}></Grid>
            <Grid item xs={7}> */}
              <Accordion style={{marginBottom:"6px"}}>
                <AccordionSummary
                  expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
                >
                  <div className="icon">
                    {/* <FontAwesomeIcon icon={faBullhorn} />{" "} */}
                  </div>
                  <br />
                  <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                    <p className="links">{so.title}</p>
                  </div>
                </AccordionSummary>
                <AccordionDetails style={{ display: "block"}}>
                  <p className="text"><pre>{so.description}</pre></p>
                </AccordionDetails>
                <Link
                  eventKey="Services"
                  className="btn btn-outline-success"
                  to={{
                    pathname: "/services",
                    state: {
                      serviceOwnerId: so.idInt,
                      eventKey: "Services",
                      eventKey: so.title,
                    },
                  }}
                >
                  {" "}
                  Read More
                </Link>
              </Accordion>
           {/*  </Grid>
            <Grid item xs={3}></Grid> */}
          </>
        )) : null}
      {/* </Grid> */}
    </div>
  );
}
