import { AddAlert } from "@mui/icons-material";
import { Snackbar } from "@mui/material";
import { useRouter } from "next/router";
import React from "react";
import { Button, Card, Col } from "react-bootstrap";
import { saveInterest } from "../../api/businessInterest";

export default function MMCompanyCol({
  sectors,
  businessTypes,
  company,
  loadUserMatches,
  userMatches,
  expanded = false,
  col = 6,
  showTitle = false,
  parentId = null,
  getLocal,
}) {
  const history = useRouter();
  var token = localStorage.getItem("access_token");
  const [showDetail, setShowDetail] = React.useState(expanded);
  React.useEffect(() => {
    setShowDetail(expanded);
  }, [expanded]);

  const [alreadyAdded, setAlreadyAdded] = React.useState(false);
  React.useEffect(() => {
    setAlreadyAdded(isAlreadyAdded());
  }, [company, userMatches]);

  const [source, setSource] = React.useState(null);
  React.useEffect(() => {
    setSource(getSource());
  }, [company, userMatches]);

  function getCompanySectors(parentId) {
    return sectors.filter(
      (s) =>
        s.parentId == parentId &&
        company.sectors.$values.find((bs) => bs.sectorId == s.id)
    );
  }
  function getCompanyBusinessTypeProducts(businessTypeId) {
    return company.details.$values.filter(
      (s) => s.businessType == businessTypeId
    );
  }
  function addContact() {
    return token
      ? saveInterest(company.id)
          .then((data) => {
            if (data.data.saved) {
              setErrorMsg("Saved");
              showNotification("tr");
              loadUserMatches();
            } else if (data.data.erorMsg) {
              setErrorMsg(data.data.erorMsg);
              showNotification("tr");
            }
          })
          .catch((error) => {
            showNotification("tr");
          })
      : history.push(`/login?redirect=/matchMaking?subSectorId=${parentId}`);
  }
  function isAlreadyAdded() {
    return userMatches && userMatches.find((u) => u.sourceId == company.id);
  }
  function getSource() {
    var match = userMatches
      ? userMatches.find((u) => u.sourceId == company.id)
      : null;
    return match ? match.source : null;
  }
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(null);
  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMsg(null);
          }, 5000);
        }
        break;
      default:
        break;
    }
  };

  const SectorList = ({ps}) => {
    return getCompanySectors(ps.id).length > 0 ? (
      <>
        <li>{ps.name}</li>
        <ul
          style={{
            listStyle: "revert",
            padding: "4px 0 0 19px",
          }}
        >
          {getCompanySectors(ps.id).map((ass, index) => (
            <li key={index}>{ass.name}</li>
          ))}
        </ul>
      </>
    ) : null;
  };
  const BusinessTypeList = ({bt}) => {
    return getCompanyBusinessTypeProducts(bt.id).length > 0 ? (
      <>
        <li>{bt.name}</li>
        <ul
          style={{
            listStyle: "revert",
            padding: "4px 0 0 19px",
          }}
        >
          {getCompanyBusinessTypeProducts(bt.id).map((ass, index) => (
            <li key={index}>
              {ass.productType.name}:- {ass.description}
            </li>
          ))}
        </ul>
      </>
    ) : null;
  };
  return (
    <>
      <Col xs={col} style={{ marginBottom: "10px" }}>
        <Card>
          {showTitle ? <Card.Header>{company.name}</Card.Header> : null}
          <Card.Body>
            {company.description}
            {alreadyAdded && source ? (
              <>
                <br />
                <br />
                <strong>
                  {getLocal("Name")}: {source.name}
                  <br />
                  {getLocal("Telephone")}: {source.telephone}
                  <br />
                  {getLocal("Email")}: {source.email}
                  {source.address ? (
                    <>
                      <br />
                      Address: {source.address}
                    </>
                  ) : null}
                </strong>
              </>
            ) : null}
            <br />
            {businessTypes &&
            showDetail &&
            company.details &&
            company.details.$values ? (
              <ul style={{ listStyle: "inherit", padding: "0 0 0 19px" }}>
                {businessTypes.map((bt, index) => (
                  <BusinessTypeList bt={bt} key={index} />
                ))}
              </ul>
            ) : null}
            <br />
            {sectors &&
            showDetail &&
            company.sectors &&
            company.sectors.$values ? (
              <ul style={{ listStyle: "inherit", padding: "0 0 0 19px" }}>
                {sectors
                  .filter((s) => !s.parentId)
                  .map((ps, index) => (
                    <SectorList ps={ps} key={index} />
                  ))}
              </ul>
            ) : null}
          </Card.Body>
          {!showTitle ? (
            <Card.Footer>
              <Button
                type="submit"
                style={{ margin: "0", float: "left" }}
                variant="outline-secondary"
                size="sm"
                id="button-addon2"
                onClick={() => setShowDetail(!showDetail)}
              >
                {showDetail ? "Less" : "More"}
              </Button>
              <Button
                type="submit"
                style={{
                  margin: "0",
                  backgroundColor: alreadyAdded ? "black" : "orange",
                }}
                variant="outline-secondary"
                size="sm"
                id="button-addon2"
                onClick={() => addContact()}
                disabled={alreadyAdded}
              >
                {alreadyAdded ? "Already Added" : "Contact"}
              </Button>
            </Card.Footer>
          ) : null}
        </Card>
      </Col>
      <Snackbar
        place="tr"
        color="danger"
        icon={AddAlert}
        message={errorMsg || !error ? errorMsg : "Failed to Save"}
        open={error}
        onClose={() => setError(false)}
      />
    </>
  );
}
