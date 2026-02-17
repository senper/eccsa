import React, { useRef } from "react";
import "bootstrap/dist/css/bootstrap.css";
import { Grid } from "@material-ui/core";
import { Link, useLocation } from "react-router-dom";
import "./Services.css";
import LegalProfilesList from "../LegalProfile/LegalProfileList";
import ExportAgreementsList from "../ExportAgreement/ExportAgreementList";
import ServiceRequestsList from "../ServiceRequest/ServiceRequestList";
import SurveyList from "../Survey/SurveyList";
import Breadcrumbs from "@material-ui/core/Breadcrumbs";
import { makeStyles } from "@material-ui/core/styles";
import UserContactsList from "../UserContacts/UserContactList";
import MembershipInfo from "../Membership/MembershipInfo";
import { useSelector } from "react-redux";
import store from "../../store";
import { loadUserFromStorage } from "../../services/oauth";
import UserMatchesList from "../MatchMaking/UserMatchesList";
import UserBusinessInterest from "../MatchMaking/UserBusinessInterest";
import ResourceRequestsList from "../Resources/ResourceRequestList";
const useStyle = makeStyles((theme) => ({
  breadcrumb: {
    backgroundColor: "#fff",
    padding: "10px",
    marginLeft: "100px",
  },
}));
export default function MyServices() {
  window.scrollTo({
    top: 0,
    behavior: "smooth",
  });
  var token = localStorage.getItem("access_token");
  const [userToken, setToken] = React.useState(token);
  const [user, setUser] = React.useState(
    useSelector((state) => state.auth.user)
  );
  React.useEffect(() => {
    if ((!user || !user.id) && token) {
      setToken(token);
      loadUserFromStorage(store).then((userInput) => {
        token = localStorage.getItem("access_token");
        setToken(token);
        setUser(userInput ? userInput.data : null);
      });
    } else if (!token) {
      setUser({});
    }
  }, [token]);
  const classes = useStyle();
  const location = useLocation();
  let navToValue = "";
  if (location.state) {
    const { navTo } = location.state;
    navToValue = navTo;
  }
  const selectPage = () => {
    let result = <></>;
    if (navToValue == "request" || navToValue == "businessInterest" || navToValue == "") {
      result = (
        <>
          {result} <ServiceRequestsList expanded={navToValue == "request"} />
          <ResourceRequestsList expanded={navToValue == "request"} />
          <UserMatchesList expanded={navToValue == "request"}/>
          <UserBusinessInterest expanded={navToValue == "request" || navToValue == "businessInterest"}/>
        </>
      );
    }
    if (user && user.businessAccount) {
      if (navToValue == "survey" || navToValue == "") {
        result = (
          <>
            {result} <SurveyList expanded={navToValue == "survey"} />
          </>
        );
      }
      if (navToValue == "legalProfile" || navToValue == "") {
        result = (
          <>
            {result}{" "}
            <LegalProfilesList expanded={navToValue == "legalProfile"} />
          </>
        );
      }
      if (navToValue == "exportAgreement" || navToValue == "") {
        result = (
          <>
            {result}{" "}
            <ExportAgreementsList expanded={navToValue == "exportAgreement"} />
          </>
        );
      }
      if (navToValue == "contacts" || navToValue == "") {
        result = (
          <>
            {result} <UserContactsList expanded={navToValue == "contacts"} />
          </>
        );
      }
      if (navToValue == "membership" || navToValue == "") {
        result = (
          <>
            {result} <MembershipInfo expanded={navToValue == "membership"} />
          </>
        );
      }
    }

    return result;
  };
  const selectPageLabel = () => {
    if (navToValue == "request" || navToValue == "businessInterest") {
      return "Requested Services";
    } else if (navToValue == "survey") {
      return "Surveys";
    } else if (navToValue == "legalProfile") {
      return "Legal Documents";
    } else if (navToValue == "exportAgreement") {
      return "Export Agreements";
    } else if (navToValue == "contacts") {
      return "Contacts";
    } else if (navToValue == "membership") {
      return "Membership";
    }
  };

  return (
    <>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrumb}>
          <Link as={Link} to="/myServices" eventKey="myServices">
            My Services
          </Link>
          <a href="javascript:void(0)">{selectPageLabel()}</a>
        </Breadcrumbs>
      </Grid>
      <Grid item xs={1} md={1}></Grid>
      <Grid item xs={10} md={10}>
        <Grid container spacing={0}>
          {selectPage()}
        </Grid>
      </Grid>
    </>
  );
}
