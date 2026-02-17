import { useRouter } from "next/router";
import React from "react";
import { useSelector } from "react-redux";
import { Grid } from "@mui/material";
import { CommonBreadcrumb } from "../../Components/Layout/CommonBreadcrumb";
import { getLocalByKey } from "../api/menu";
import ServiceRequestsList from "../../Components/ServiceRequest/ServiceRequestsList";
import ResourceRequestsList from "../../Components/Resources/ResourceRequestList";
import UserMatchesList from "../../Components/MatchMaking/UserMatchesList";
import UserBusinessInterest from "../../Components/MatchMaking/UserBusinessInterest";
import SurveyList from "../../Components/Survey/SurveyList";
import LegalProfilesList from "../../Components/LegalProfile/LegalProfileList";
import ExportAgreementsList from "../../Components/ExportAgreement/ExportAgreementList";
import UserContactsList from "../../Components/UserContacts/UserContactList";
import MembershipInfo from "../../Components/Membership/MembershipInfo";
import Custom404 from "../404";
import Login from "../login";

export default function MyServices({ translations }) {
  const [user, setUser] = React.useState(
    useSelector((state) => state.auth.user)
  );

  window.scrollTo({
    top: 0,
    behavior: "smooth",
  });
  const router = useRouter();
  if (!user) {
    //return <Custom404 statusCode={404} />;
    return <Login translations={translations} />;
  }
  const { locale } = router;
  const { navTo } = router.query;

  const selectPage = () => {
    let result = <></>;
    if (navTo == "request" || navTo == "businessInterest" || !navTo) {
      result = (
        <>
          {result}{" "}
          <ServiceRequestsList
            expanded={navTo == "request"}
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
          <ResourceRequestsList
            expanded={navTo == "request"}
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
          <UserMatchesList
            expanded={navTo == "request"}
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
          <UserBusinessInterest
            expanded={navTo == "request" || navTo == "businessInterest"}
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
        </>
      );
    }
    if (user && user.businessAccount) {
      if (navTo == "survey" || !navTo) {
        result = (
          <>
            {result}{" "}
            <SurveyList
              expanded={navTo == "survey"}
              getLocal={getLocal}
              getLocalByVal={getLocalByVal}
            />
          </>
        );
      }
      if (navTo == "legalProfile" || !navTo) {
        result = (
          <>
            {result}{" "}
            <LegalProfilesList
              expanded={navTo == "legalProfile"}
              getLocal={getLocal}
              getLocalByVal={getLocalByVal}
            />
          </>
        );
      }
      if (navTo == "exportAgreement" || !navTo) {
        result = (
          <>
            {result}{" "}
            <ExportAgreementsList
              expanded={navTo == "exportAgreement"}
              getLocal={getLocal}
              getLocalByVal={getLocalByVal}
            />
          </>
        );
      }
      if (navTo == "contacts" || !navTo) {
        result = (
          <>
            {result}{" "}
            <UserContactsList
              expanded={navTo == "contacts"}
              getLocal={getLocal}
              getLocalByVal={getLocalByVal}
            />
          </>
        );
      }
      if (navTo == "membership" || !navTo) {
        result = (
          <>
            {result}{" "}
            <MembershipInfo
              expanded={navTo == "membership"}
              getLocal={getLocal}
              getLocalByVal={getLocalByVal}
            />
          </>
        );
      }
    }

    return result;
  };
  const selectPageLabel = () => {
    if (navTo == "request" || navTo == "businessInterest") {
      return "Requested Services";
    } else if (navTo == "survey") {
      return "Surveys";
    } else if (navTo == "legalProfile") {
      return "Legal Documents";
    } else if (navTo == "exportAgreement") {
      return "Export Agreements";
    } else if (navTo == "contacts") {
      return "Contacts";
    } else if (navTo == "membership") {
      return "Membership";
    } else {
      return "All";
    }
  };

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  const breadcrumbLinks = () => [
    {
      title: getLocal("My Services"),
      path: "/myServices",
    },
    {
      title: selectPageLabel(),
      path: "#",
    },
  ];
  return (
    <>
      {
        <style>
          {`
          .faicon, .icon {
              text-shadow: 3px 3px 4px #ccc;
              text-align: center;
              text-decoration: none;
          }
          .icon {
              color: #000;
              font-size: 2.5em;
          }`}
        </style>
      }
      <div className="paddedMainDiv">
        <Grid item xs={12} md={12}>
          <CommonBreadcrumb links={breadcrumbLinks()} />
        </Grid>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={12} md={10}>
          <Grid container spacing={0}>
            {selectPage()}
          </Grid>
        </Grid>
      </div>
    </>
  );
}
