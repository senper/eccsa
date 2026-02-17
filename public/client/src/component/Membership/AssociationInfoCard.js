import React from "react";
import {
  Grid,
  Accordion,
  AccordionSummary,
  AccordionDetails,
} from "@material-ui/core";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { Link } from "react-router-dom";
import { Col, Row } from "react-bootstrap";

export default function AssociationInfoCard({ association }) {
  var photoStyle ={
    maxHeight: "170px",
    maxWidth: "170px",
    minHeight: "170px",
    minWidth: "170px",
  };
  const formateProfile = () => {
    let profile = association.profile;    
    profile = profile.replace('src="/members/photo/img', `src="${process.env.REACT_APP_API_URL}/members/photo/img`);
    return profile;
  }
  return (
    <>
      <Grid item xs={12}>
        <Accordion defaultExpanded={true}>
          <AccordionSummary expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}>
            <div className="icon">
              <img
                alt={association.nameEng}
                src={`${process.env.REACT_APP_API_URL}/${association.logo}`}
                style={{
                  maxHeight: "90px",
                  maxWidth: "90px",
                  minHeight: "30px",
                  minWidth: "30px",
                }}
              />
            </div>
            <br />
            <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
              <p className="links">
                {association.nameEng} {association.nameLocal}
              </p>
            </div>
          </AccordionSummary>
          <AccordionDetails style={{ display: "block" }}>
            <Row>
              <Col sm="9">
                <p className="text">
                  Type: <strong>{association.associationType}</strong>
                </p>
                <p className="text">
                  Level: <strong>{association.associationLevel}</strong>
                </p>
                <p className="text">
                  Location:{" "}
                  <strong>
                    {association.location.nameEng}{" "}
                    {association.location.nameLocal}
                  </strong>
                </p>
                {association.parent ? (
                  <p className="text">
                    Member of:{" "}
                    <strong>
                      {association.parent.nameEng}{" "}
                      {association.parent.nameLocal}
                    </strong>
                  </p>
                ) : null}
                <br/>
                <strong style={{paddingLeft: "10px", fontSize: "20px"}}>Profile</strong><br/>
                <div style={{border: "1px solid #ccc", borderRadius: "16px", padding: "0.01em 16px" }} dangerouslySetInnerHTML={{__html: formateProfile()}} />
              </Col>
              <Col sm="3">
                <img
                  src={`${process.env.REACT_APP_API_URL}${association.boardChairman.logo}`}
                  alt="Board Chairperson"
                  style={photoStyle}
                />{" "}
                <br />
                {association.boardChairman.positionName}
                <br />
                <hr />
                <img
                  src={`${process.env.REACT_APP_API_URL}${association.ceo.logo}`}
                  alt="Secretary General"
                  style={photoStyle}
                />
                <br />
                {association.ceo.positionName}
              </Col>
            </Row>
          </AccordionDetails>
        </Accordion>
      </Grid>
    </>
  );
}
