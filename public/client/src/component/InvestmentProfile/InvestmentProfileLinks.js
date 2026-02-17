import React from "react";

import { Link } from "react-router-dom";
import { Card } from "react-bootstrap";

export default function InvestmentProfileLinks({
  investmentProfiles,
  loadInvestmentProfileDetail,
  selectedInvestmentProfile,
  profileListTitle,
}) {
  return (
    <Card text="white" style={{ width: "18rem" }}>
      <Card.Header>{profileListTitle}</Card.Header>
      {profileListTitle != "Ethiopia" ? (
        <Card.Body>
          <Card.Text>
            {investmentProfiles.map((ass) => (
              <li
                style={{
                  whiteSpace: "nowrap",
                  overflow: "hidden",
                  maxWidth: "270px",
                }}
              >
                <Link
                  title={ass.name}
                  style={
                    selectedInvestmentProfile &&
                    selectedInvestmentProfile.id == ass.id
                      ? { color: "black" }
                      : {}
                  }
                  onClick={() => loadInvestmentProfileDetail(ass.id)}
                >
                  {ass.name}
                </Link>
              </li>
            ))}
          </Card.Text>
        </Card.Body>
      ) : null}
    </Card>
  );
}
