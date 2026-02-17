import React from "react";
import {
  Grid,
  Accordion,
  AccordionSummary,
  AccordionDetails,
} from "@material-ui/core";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { Col, Row } from "react-bootstrap";

export default function InvestmentProfileDetail({ profileEntity }) {
  const formateProfile = () => {
    let profile = profileEntity.profile;
    if (profile) {
      profile = profile.replace(
        'src="/members/photo/img',
        `src="${process.env.REACT_APP_API_URL}/members/photo/img`
      );
      return profile;
    }
  };
  return (
    <>
      <Grid item xs={12}>
        <Accordion defaultExpanded={true}>
          <AccordionSummary expandIcon={<ExpandMoreIcon style={{marginBottom: "0px"}}></ExpandMoreIcon>} style={{marginBottom: "0px"}}>
            <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
              <p className="links" style={{marginBottom: "0px"}}>{profileEntity.name}</p>
            </div>
          </AccordionSummary>
          <AccordionDetails style={{ display: "block", marginTop: '-30px' }}>
            <Row>
              <Col>
                <strong style={{ paddingLeft: "10px", fontSize: "20px" }}>
                  Profile
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
            </Row>
          </AccordionDetails>
        </Accordion>
      </Grid>
    </>
  );
}
