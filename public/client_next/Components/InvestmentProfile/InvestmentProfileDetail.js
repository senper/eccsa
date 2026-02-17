import React from 'react';
import {
  Grid,
  Accordion,
  AccordionSummary,
  AccordionDetails,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { Col, Row } from 'react-bootstrap';

export default function InvestmentProfileDetail({
  profileEntity,
  getLocal,
  getLocalByVal,
}) {
  const formateProfile = () => {
    let profile = getLocalByVal(
      profileEntity.profile,
      profileEntity.profileLocal
    );
    if (profile) {
      profile = profile.replaceAll(
        'src="/members/photo/img',
        `src="${process.env.NEXT_PUBLIC_API_URL}/members/photo/img`
      );      
      profile = profile.replaceAll(
        'href="/members/documents/attachment',
        `href="${process.env.NEXT_PUBLIC_API_URL}/members/documents/attachment`
      );
      return profile;
    }
  };
  return (
    <>
      <Grid item xs={12}>
        <Accordion defaultExpanded={true}>
          <AccordionSummary
            expandIcon={
              <ExpandMoreIcon style={{ marginBottom: '0px' }}></ExpandMoreIcon>
            }
            style={{ marginBottom: '0px' }}
          >
            <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
              <p className="links" style={{ marginBottom: '0px' }}>
                {getLocalByVal(profileEntity.name, profileEntity.nameLocal)}
              
              </p>
            </div>
          </AccordionSummary>
          <AccordionDetails style={{ display: 'block', marginTop: '-30px' }}>
            <Row>
              <Col>
                <strong style={{ paddingLeft: '10px', fontSize: '20px' }}>
                  {getLocal('Profile')}
                </strong>
                <br />
                <div
                  style={{
                    border: '1px solid #ccc',
                    borderRadius: '16px',
                    padding: '0.01em 16px',
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
