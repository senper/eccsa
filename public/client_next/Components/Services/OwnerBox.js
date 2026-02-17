import {
  Accordion,
  AccordionDetails,
  Grid,
} from "@mui/material";
import { Col, Nav, Row, Tab } from "react-bootstrap";
import { TabPan } from "./OwerBoxTabPan";
import { OwnerBoxSummary } from "./OwnerBoxSummary";

export const OwnerBox = ({
  so,
  serviceOwnerId,
  serviceTypes,
  selectedServiceTypeId,
  lang,
  getLocal
}) => {
  const hasServiceTypes = () => {
    return (
      serviceTypes &&
      serviceTypes.filter((s) => s.serviceOwner === so.id).length > 0
    );
  };
  const getFirstOwnerServiceType = () => {
    return serviceTypes
      .sort(compare3)
      .filter((s) => s.serviceOwner === so.id)[0];
  };
  const getOwnerServiceTypes = () => {
    return serviceTypes.sort(compare3).filter((s) => s.serviceOwner === so.id);
  };
  function compare3(a, b) {
    if (!selectedServiceTypeId) {
      return 0;
    }
    if (selectedServiceTypeId === a.id.toString()) {
      return -1;
    }
    if (selectedServiceTypeId === b.id.toString()) {
      return 1;
    }
    return 0;
  }
  const getLocalByVal = (eng, local) => {
    return lang == "en" || !local ? eng : local;
  };
  const NavItem = ({ s }) => {
    return (
      <Nav.Item>
        <Nav.Link eventKey={s.id}>
          {s.name}
        </Nav.Link>
      </Nav.Item>
    );
  };
  return (
    <>
      <Grid item xs={11}>
        <Accordion
          id={`accordian${so.$id}`}
          defaultExpanded={so.idInt === serviceOwnerId}
        >
          <OwnerBoxSummary
            so={so}
            getLocalByVal={getLocalByVal}
            getLocal={getLocal}
          />
          <AccordionDetails style={{ display: "block", marginTop: "-15px" }}>
            <Row style={{ paddingTop: "0px" }}>
              <Col sm={12}>
                <pre>{so.description}</pre>
                {hasServiceTypes() ? (
                  <Tab.Container
                    id="left-tabs-example"
                    defaultActiveKey={
                      getFirstOwnerServiceType()
                        ? getFirstOwnerServiceType().id
                        : ""
                    }
                  >
                    <Row style={{ paddingTop: "10px" }}>
                      <Col sm={3}>
                        <Nav variant="pills" style={{ display: "block" }}>
                          {getOwnerServiceTypes().map((s, index) => (
                            <NavItem s={s} key={index} />
                          ))}
                        </Nav>
                        <br />
                        <br />
                        <br />
                      </Col>
                      <Col sm={9}>
                        <Tab.Content>
                          {serviceTypes
                            ? serviceTypes
                                .sort(compare3)
                                .map((s, index) => (
                                  <TabPan
                                    s={s}
                                    so={so}
                                    key={index}
                                    getLocalByVal={getLocalByVal}
                                    getLocal={getLocal}
                                  />
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
        </Accordion>
      </Grid>
    </>
  );
};
