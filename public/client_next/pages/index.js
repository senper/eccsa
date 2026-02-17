import { Grid, Paper } from "@mui/material";
import { useRouter } from "next/router";
import { Col, Row } from "react-bootstrap";
import BusinessInfoHighlight from "../Components/BusinessInformation/BusinessInfoHighlight";
import EventsSection from "../Components/Event/EventsSection";
import BusinessInterestHighlight from "../Components/MatchMaking/BusinessInterestHighlight";
import BecomeAMemberInfo from "../Components/Membership/BecomeAMemberInfo";
import NewsHighlight from "../Components/News/NewsHighlight";
import Partners from "../Components/Partners/Partners";
import ServiceModules from "../Components/Services/ServiceModules";
import HomeSlider from "../Components/slider/HomeSlider";
import { getLocalByKey } from "./api/menu";

export default function Home({ translations }) {
  const location = useRouter();
  const { locale } = location;
  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  return (
    <Grid
      container
      spacing={2}
      style={
        {
          // width: "100%",
          // background: "#fff",
          //padding: "10px",
          // paddingTop: "0",
        }
      }
    
    >
      <HomeSlider getLocal={getLocal} getLocalByVal={getLocalByVal} />
      <ServiceModules getLocal={getLocal} getLocalByVal={getLocalByVal} />
      <NewsHighlight getLocal={getLocal} getLocalByVal={getLocalByVal} />
      <EventsSection getLocal={getLocal} getLocalByVal={getLocalByVal} />
      <Grid item xs={12} md={12}>
        <Row>
          <Col md={6}>
            <BecomeAMemberInfo
              getLocal={getLocal}
              getLocalByVal={getLocalByVal}
            />
          </Col>
          <Col md={6}>
            <BusinessInterestHighlight
              getLocal={getLocal}
              getLocalByVal={getLocalByVal}
            />
            <br/>
            <br/>
            <BusinessInfoHighlight
              getLocal={getLocal}
              getLocalByVal={getLocalByVal}
            />
          </Col>
        </Row>
      </Grid>
      <Partners getLocal={getLocal} getLocalByVal={getLocalByVal} />
    </Grid>
  );
}
