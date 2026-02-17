import { Accordion, AccordionDetails, AccordionSummary } from "@mui/material";
import Link from "next/link";
import React from "react";
import { Row, Col } from "react-bootstrap";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";

export default function ExistingMemberships({
  memberships,
  loading,
  getLocal,
  getLocalByVal,
}) {
  return <>
    {memberships.length <= 0 && !loading ? (
      <h3>{getLocal("No Membership Information found")}</h3>
    ) : null}
    {memberships
      ? memberships.map((membership, index) => (
          <Accordion defaultExpanded={false} key={index}>
            <AccordionSummary expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}>
              <div className="icon">
                {/* <FontAwesomeIcon icon={faUsers} /> */}
              </div>
              <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                <p className="links" style={{ margin: "0px" }}>
                  {" "}
                  {getLocalByVal(membership.association, membership.associationLocal)}
                </p>
              </div>
            </AccordionSummary>
            <AccordionDetails style={{ display: "block" }}>
              <Row className="g-12">
                <Col md>
                  {getLocal("Code")}: <strong>{membership.code}</strong>
                </Col>
                <Col md>
                  {getLocal("Member Since")}:{" "}
                  <strong>{membership.memberSince}</strong>
                </Col>
              </Row>
              <Row className="g-12">
                <Col md>
                  {getLocal("Renewed On")}:{" "}
                  <strong>{membership.renewedOn}</strong>
                </Col>
                <Col md>
                  {getLocal("Expires On")}:{" "}
                  <strong>{membership.expiresOn}</strong>
                  {membership.renewalStatusValue != null ? (
                    <>
                      <br /> {getLocal("New Renewal Application Status")}:{" "}
                      <strong>{membership.renewalStatus}</strong>
                    </>
                  ) : (
                    (<Link
                      href={`/myServices/membershipApplyForm?associationId=${membership.associationId}&type=2`}
                      style={{ float: "none", margin: "5px 0px 0 10px" }}
                      className="btn-sm btn btn-success">

                      {getLocal("Apply for Renewal")}

                    </Link>)
                  )}
                </Col>
              </Row>
              <Row className="g-12">
                <Col md>
                  {getLocal("Invoice No")}:{" "}
                  <strong>{membership.invoiceNo}</strong>
                </Col>
                <Col md>
                  {getLocal("Receipt No")}:{" "}
                  <strong>{membership.receiptNo}</strong>
                </Col>
              </Row>
              <Row className="g-12">
                <Col md>
                  {getLocal("Fee")}: <strong>{membership.fee}</strong>
                </Col>
              </Row>
              <Row className="g-12">
                <Col md>
                  {getLocal("Status")}: <strong>{membership.status}</strong>
                </Col>
              </Row>
            </AccordionDetails>
          </Accordion>
        ))
      : null}
  </>;
}
