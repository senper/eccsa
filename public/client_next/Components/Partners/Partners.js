import { Grid } from "@mui/material";
import React from "react";
import { Col, Container, Row } from "react-bootstrap";
//import { getMediaType } from "../../pages/api/menu";

export default function Partners({ getLocal, getLocalByVal }) {
  const partners = [
    "client-1.png",
    "client-2.jpg",
    "client-3.png",
    "client-4.jpg",
    "Irish Aid Logo.jpg",
  ];

  /* const [content, setContent] = React.useState([]);
  React.useEffect(() => {
    getMediaType(9).then((data) => {
      if (
        data.data &&
        data.data.contents &&
        data.data.contents.$values &&
        data.data.contents.$values.length > 0
      ) {
        setContent(data.data.contents.$values[0]);
        // console.log(data.data.contents.$values[0]);
      }
    });
  }, []); 
  const formateProfile = () => {
    if (!content) return "";
    let profile = getLocalByVal(content.content, content.contentLocal);
    if (!profile) return "";
    profile = profile.replaceAll(
      'src="/pr/text_media',
      `src="${process.env.NEXT_PUBLIC_API_URL}/pr/text_media`
    );
    return profile;
  };*/
  return (
    <Grid item xs={12} md={12}>
      <div className="bg">{getLocal("Our Partners")}</div>
      <footer className="text-center text-white">
        <Container className="p-4">
          <Row>
            {/* <div dangerouslySetInnerHTML={{ __html: formateProfile() }} /> */}
            {partners.map((v, index) => (
              <Col lg={2} md={4} key={index}>
                <div
                  className="bg-image hover-overlay ripple shadow-1-strong rounded"
                  data-ripple-color="light"
                >
                  <img
                    style={{ height: "80px", maxWidth: "150px" }}
                    src={"./images/clients/" + v}
                    className="w-100"
                    alt={v}
                  />
                </div>
              </Col>
            ))}
          </Row>
        </Container>
      </footer>
    </Grid>
  );
}
