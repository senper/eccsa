import React from 'react';
import { Card } from 'react-bootstrap';
import { Link } from '@mui/material';
export default function InvestmentProfileLinks({
  investmentProfiles,
  loadInvestmentProfileDetail,
  selectedInvestmentProfile,
  profileListTitle,
  getLocal,
  getLocalByVal,
}) {
  return (
    <Card text="white" style={{ width: '18rem', marginBottom: "50px" }}>
      <Card.Header style={{ backgroundColor: 'green' }}>
        {getLocal(profileListTitle)}
      </Card.Header>
      {profileListTitle != 'Ethiopia' ? (
        <Card.Body>
          <Card.Text>
            {investmentProfiles.map((ass) => (
              <li key={ass.id} className="li-links">
                <a
                  className="text-primary"
                  title={ass.name}
                  style={
                    selectedInvestmentProfile &&
                    selectedInvestmentProfile.id == ass.id
                      ? { color: 'black' }
                      : {}
                  }
                  onClick={() => loadInvestmentProfileDetail(ass.id)}
                >
                  {getLocalByVal(ass.name, ass.nameLocal)}
                </a>
              </li>
            ))}
          </Card.Text>
        </Card.Body>
      ) : null}
    </Card>
  );
}
