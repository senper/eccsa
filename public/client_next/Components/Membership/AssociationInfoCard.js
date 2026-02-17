import React from "react";
import {
  Grid,
  Accordion,
  AccordionSummary,
  AccordionDetails,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { Col, Row } from "react-bootstrap";

export default function AssociationInfoCard({
  association,
  getLocal,
  getLocalByVal,
}) {
  var photoStyle = {
    maxHeight: "170px",
    maxWidth: "170px",
    minHeight: "170px",
    minWidth: "170px",
  };
  const formateProfile = () => {
    let profile = getLocalByVal(association.profile, association.profileLocal);
    profile = profile.replaceAll(
      'src="/members/photo/img',
      `src="${process.env.NEXT_PUBLIC_API_URL}/members/photo/img`
    );
    return profile;
  };
  return (
    <>
      <Grid item xs={12}>
        <Accordion defaultExpanded={true}>
          <AccordionSummary expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}>
            <div className="icon">
              <img
                alt={association.nameEng}
                src={`${process.env.NEXT_PUBLIC_API_URL}/${association.logo}`}
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
                  {getLocal("Type")}:{" "}
                  <strong>{getLocal(association.associationType)}</strong>
                </p>
                <p className="text">
                  {getLocal("Level")}:{" "}
                  <strong>{getLocal(association.associationLevel)}</strong>
                </p>
                <p className="text">
                  {getLocal("Location")}:{" "}
                  <strong>
                    {getLocalByVal(
                      association.location.nameEng,
                      association.location.nameLocal
                    )}
                  </strong>
                </p>
                {association.parent ? (
                  <p className="text">
                    {getLocal("Member of")}:{" "}
                    <strong>
                      {getLocalByVal(
                        association.parent.nameEng,
                        association.parent.nameLocal
                      )}
                    </strong>
                  </p>
                ) : null}
                <br />
                <strong style={{ paddingLeft: "10px", fontSize: "20px" }}>
                  {getLocal("Profile")}
                </strong>
                <br />
                <div
                  style={{
                    border: "1px solid #ccc",
                    borderRadius: "16px",
                    padding: "0.01em 16px",
                  }}
                  dangerouslySetInnerHTML={{ __html: formateProfile() }}
                />
              </Col>
              <Col sm="3">
                <img
                  src={`${process.env.NEXT_PUBLIC_API_URL}${association.boardChairman.logo}`}
                  alt={getLocal("Board Chairperson")}
                  style={photoStyle}
                />{" "}
                <br />
                {getLocal(association.boardChairman.positionName)}
                <br />
                <hr />
                <img
                  src={`${process.env.NEXT_PUBLIC_API_URL}${association.ceo.logo}`}
                  alt={getLocal("Secretary General")}
                  style={photoStyle}
                />
                <br />
                {getLocal(association.ceo.positionName)}
              </Col>
            </Row>
          </AccordionDetails>
        </Accordion>
      </Grid>
    </>
  );
}
