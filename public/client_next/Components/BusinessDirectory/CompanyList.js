import React from "react";
import { Card, Col, Row } from "react-bootstrap";

export default function CompanyList({
  sectors,
  companies,
  selectedSector,
  loading,
  getLocal,
}) {
  function getImgSrc(id, extension) {
    const fileName = id.substr(id.length - 15).replace("-", "");
    return `${process.env.NEXT_PUBLIC_API_URL}/directory/${fileName}${extension}`;
  }
  function getCompanySectors(parentId, company) {
    return sectors.filter(
      (s) =>
        s.parentId == parentId &&
        company.businessSectors.$values.find((bs) => bs.sectorId == s.id)
    );
  }
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
      <Row>
        {companies
          ? companies.map((d, index) => (
              <Col xs={12} style={{ marginBottom: "10px" }} key={index}>
                <Card>
                  <Card.Header
                    style={{
                      backgroundColor: "#f5f5f5",
                      color: "black",
                      fontWeight: "bold",
                      fontSize: "16px",
                    }}
                  >
                    {d.name}
                  </Card.Header>
                  <Card.Body>
                    {d.extension ? (
                      <img
                        alt={"Company Logo"}
                        className="directory_img"
                        src={getImgSrc(d.id, d.extension)}
                        onClick={() => {
                          window.open(getImgSrc(d.id, d.extension), "_blank");
                        }}
                      />
                    ) : null}
                    {d.telephone && d.telephone.trim() != "" ? (
                      <>
                        {getLocal("Tel")}: {d.telephone}
                        <br />
                      </>
                    ) : null}
                    {d.mobile && d.mobile ? (
                      <>
                        {getLocal("Mob")}: {d.mobile}
                        <br />
                      </>
                    ) : null}
                    {d.fax && d.fax ? (
                      <>
                        {getLocal("Fax")}: {d.fax}
                        <br />
                      </>
                    ) : null}
                    {d.email && d.email ? (
                      <>
                        {getLocal("Email")}: {d.email}
                        <br />
                      </>
                    ) : null}
                    {d.poBox && d.poBox ? (
                      <>
                        {getLocal("P.O.Box")}: {d.poBox}
                        <br />
                      </>
                    ) : null}
                    {d.web && d.web ? (
                      <>
                        {getLocal("Web")}: {d.web}
                        <br />
                      </>
                    ) : null}
                  </Card.Body>
                  {sectors && d.businessSectors && d.businessSectors.$values ? (
                    <Card.Footer>
                      <ul
                        style={{ listStyle: "inherit", padding: "0 0 0 19px" }}
                      >
                        {sectors
                          .filter((s) => !s.parentId)
                          .map((ps, index1) => (
                            <div key={index1}>
                              {getCompanySectors(ps.id, d).length > 0 ? (
                                <>
                                  <li>{ps.name}</li>
                                  <ul
                                    style={{
                                      listStyle: "revert",
                                      padding: "4px 0 0 19px",
                                    }}
                                  >
                                    {getCompanySectors(ps.id, d).map(
                                      (ass, index2) => (
                                        <li key={index2}>{ass.name}</li>
                                      )
                                    )}
                                  </ul>
                                </>
                              ) : null}
                            </div>
                          ))}
                      </ul>
                    </Card.Footer>
                  ) : null}
                </Card>
              </Col>
            ))
          : null}
        {!loading && (!companies || companies.length <= 0) ? (
          <h3>NOTHING FOUND</h3>
        ) : null}
      </Row>
    </>
  );
}
