import React from "react";
import {
  Card,
  Col,
  Row,
} from "react-bootstrap";

export default function CompanyList({
    sectors, companies, selectedSector, loading
}) {
  function getImgSrc(id, extension) {
    const fileName = id.substr(id.length - 15).replace("-", "");
    return `${process.env.REACT_APP_API_URL}/directory/${fileName}${extension}`;
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
          ? companies.map((d) => (
              <Col xs={6} style={{ marginBottom: "10px" }}>
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
                        class="directory_img"
                        src={getImgSrc(d.id, d.extension)}
                      />
                    ) : null}
                    {d.telephone && d.telephone.trim() != "" ? (
                      <>
                        Tel: {d.telephone}
                        <br />
                      </>
                    ) : null}
                    {d.mobile && d.mobile ? (
                      <>
                        Mob: {d.mobile}
                        <br />
                      </>
                    ) : null}
                    {d.fax && d.fax ? (
                      <>
                        Fax: {d.fax}
                        <br />
                      </>
                    ) : null}
                    {d.email && d.email ? (
                      <>
                        Email: {d.email}
                        <br />
                      </>
                    ) : null}
                    {d.poBox && d.poBox ? (
                      <>
                        P.O.Box: {d.poBox}
                        <br />
                      </>
                    ) : null}
                    {d.web && d.web ? (
                      <>
                        Web: {d.web}
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
                          .map((ps) => (
                            <>
                              {getCompanySectors(ps.id, d).length > 0 ? (
                                <>
                                  <li>{ps.name}</li>
                                  <ul
                                    style={{
                                      listStyle: "revert",
                                      padding: "4px 0 0 19px",
                                    }}
                                  >
                                    {getCompanySectors(ps.id, d).map((ass) => (
                                      <li>{ass.name}</li>
                                    ))}
                                  </ul>
                                </>
                              ) : null}
                            </>
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
