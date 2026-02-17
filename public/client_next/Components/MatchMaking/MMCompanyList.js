import React from "react";
import { Col, Row } from "react-bootstrap";
import MMCompanyCol from "./MMCompanyCol";
export default function MMCompanyList({
  sectors,
  companies,
  selectedSector,
  businessTypes,
  loading,
  loadUserMatches,
  userMatches,
  searched,
  getLocal,
  getLocalByVal,
}) {
  const renderInfo = (first = true) => {
    const content = [];
    const length = Math.floor(companies.length / 2) + (companies.length % 2);
    const start = first ? 0 : length;
    const end = first ? length : companies.length;
    for (let i = start; i < end; i++) {
      content.push(
        <MMCompanyCol key={i}
          sectors={sectors}
          company={companies[i]}
          businessTypes={businessTypes}
          loadUserMatches={loadUserMatches}
          userMatches={userMatches}
          expanded={searched}
          col={12}
          parentId={selectedSector ? selectedSector.id : null}
          getLocal={getLocal}
        />
      );
    }

    return content;
  };
  return (
    <>
      {selectedSector ? (
        <h4>
          <span style={{ fontWeight: "bolder" }}>
            {selectedSector.parentName}
          </span>{" "}
          / {selectedSector.name}
        </h4>
      ) : null}
      <hr style={{ width: "100%" }} />
      <Row style={{ paddingTop: "0" }}>
        {companies ? (
          <>
            <Row style={{ padding: "0" }}>
              <Col sm={6}>
                <Row style={{ padding: "0" }}>{renderInfo(true)}</Row>
              </Col>
              <Col sm={6}>
                <Row style={{ padding: "0" }}>{renderInfo(false)}</Row>
              </Col>
            </Row>
          </>
        ) : null}
        {!loading && (!companies || companies.length <= 0) && (selectedSector || searched) ? (
          <h3>{getLocal("NOTHING FOUND")}</h3>
        ) : null}
      </Row>
    </>
  );
}
