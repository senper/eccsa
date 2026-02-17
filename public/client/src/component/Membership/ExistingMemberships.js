import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import {
  Accordion,
  AccordionSummary,
  AccordionDetails,
} from "@material-ui/core";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { Row, Col } from "react-bootstrap";
import { Link } from "react-router-dom";

export default function ExistingMemberships({
  memberships,
  loading,
}) {
  return (
    <>
      {memberships.length <= 0 && !loading ? (
        <h3>No Membership Information found</h3>
      ) : null}
      {memberships
        ? memberships.map((membership) => (
            <>
              <Accordion defaultExpanded={false}>
                <AccordionSummary
                  expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
                >
                  <div class="icon">
                    {/* <FontAwesomeIcon icon={faUsers} /> */}
                  </div>
                  <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                    <p className="links" style={{ margin: "0px" }}>
                      {" "}
                      {membership.association}
                    </p>
                  </div>
                </AccordionSummary>
                <AccordionDetails style={{ display: "block" }}>
                  <Row className="g-12">
                    <Col md>
                      Code: <strong>{membership.code}</strong>
                    </Col>
                    <Col md>
                      Member Since: <strong>{membership.memberSince}</strong>
                    </Col>
                  </Row>
                  <Row className="g-12">
                    <Col md>
                      Renewed On: <strong>{membership.renewedOn}</strong>
                    </Col>
                    <Col md>
                      Expires On: <strong>{membership.expiresOn}</strong>
                      {membership.renewalStatusValue != null ? (
                        <>
                          <br /> New Renewal Application Status:{" "}
                          <strong>{membership.renewalStatus}</strong>
                        </>
                      ) : (
                        <Link
                          style={{ float: "none", margin: "5px 0px 0 10px" }}
                          className="btn-sm btn btn-success"
                          to={{
                            pathname: "/membershipApplicationForm",
                            state: {
                              associationId: membership.associationId,
                              type: 2,
                            },
                          }}
                        >
                          Apply for Renewal
                        </Link>
                      )}
                    </Col>
                  </Row>
                  <Row className="g-12">
                    <Col md>
                      Invoice No: <strong>{membership.invoiceNo}</strong>
                    </Col>
                    <Col md>
                      Receipt No: <strong>{membership.receiptNo}</strong>
                    </Col>
                  </Row>
                  <Row className="g-12">
                    <Col md>
                      Fee: <strong>{membership.fee}</strong>
                    </Col>
                  </Row>
                  <Row className="g-12">
                    <Col md>
                      Status: <strong>{membership.status}</strong>
                    </Col>
                  </Row>
                </AccordionDetails>
              </Accordion>
            </>
          ))
        : null}
    </>
  );
}
