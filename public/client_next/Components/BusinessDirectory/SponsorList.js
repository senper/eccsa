import React from "react";
import { Container, Grid } from "@mui/material";
import { getAdImages } from "../../api/advert";
import { Col, Row } from "react-bootstrap";

const imgStyle = {
  borderColor: "black",
  borderStyle: "solid",
  borderWidth: "0.8px",
  maxWidth: "200px",
  width: "100%",
  height: "100%",
};

const advertHereList = ["adverheresquer.jpg", "adverheresquer2.jpg"];
const mainUrl = `${process.env.NEXT_PUBLIC_API_URL}/AdvertisingImages/`;
export default function SponsorList({ getLocal, getLocalByVal }) {
  const [pList, setPlist] = React.useState([]);
  React.useEffect(() => {
    getAdImages(5).then((data) => {
      setPlist(data.data);
    });
  }, []);
  const [gList, setGlist] = React.useState([]);
  React.useEffect(() => {
    getAdImages(6).then((data) => {
      setGlist(data.data);
    });
  }, []);
  const [sList, setSlist] = React.useState([]);
  React.useEffect(() => {
    getAdImages(7).then((data) => {
      setSlist(data.data);
    });
  }, []);
  const [bList, setBlist] = React.useState([]);
  React.useEffect(() => {
    getAdImages(8).then((data) => {
      setBlist(data.data);
    });
  }, []);
  function renderImgList(list, title, folder) {
    return (
      <>
        <div className="bg" style={{ textAlign: "center" }}>
          {getLocal(title)}
        </div>
        {list && list.$values && list.$values.length > 0 ? (
          <>
            <Grid container spacing={1}>
              <Row>
                {list.$values.map((s, index) => (
                  <Col
                    sm={6}
                    md={4}
                    lg={3}
                    key={index}
                    style={{ marginBottom: "10px" }}
                  >
                    <img
                      alt={"sponsor"}
                      style={imgStyle}
                      src={mainUrl + folder + "/" + s}
                    />{" "}
                  </Col>
                ))}
              </Row>
            </Grid>{" "}
          </>
        ) : (
          <Row>
            {advertHereList.map((s, index) => (
              <Col
                sm={6}
                md={4}
                lg={3}
                style={{ marginBottom: "10px" }}
                key={index}
              >
                <img
                  alt={"sponsor"}
                  style={imgStyle}
                  src={"/AdvertisingImages/" + s}
                />
              </Col>
            ))}
          </Row>
        )}
      </>
    );
  }
  return (
    <Container>
      {renderImgList(pList, "Platinum Sponsors", "platinum")}
      {renderImgList(gList, "Gold Sponsors", "gold")}
      {renderImgList(sList, "Silver Sponsors", "silver")}
      {renderImgList(bList, "Bronze Sponsors", "bronze")}
    </Container>
  );
}
