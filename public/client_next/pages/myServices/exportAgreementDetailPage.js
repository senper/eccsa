import { Grid } from "@mui/material";
import { useRouter } from "next/router";
import React from "react";
import { useSelector } from "react-redux";
import ExportAgreementDetail from "../../Components/ExportAgreement/ExportAgreementDetail";
import { CommonBreadcrumb } from "../../Components/Layout/CommonBreadcrumb";
import Custom404 from "../404";
import Login from "../login";
import { getLocalByKey } from "../api/menu";

export default function ExportAgreementDetailPage({ translations }) {
  const [user, setUser] = React.useState(
    useSelector((state) => state.auth.user)
  );

  const location = useRouter();
  if (!user) {
    //return <Custom404 statusCode={404} />;
    return <Login translations={translations} />;
  }
  const { locale } = location;
  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  const breadcrumbLinks = () => [
    {
      title: getLocal("Home"),
      path: "/",
    },
    {
      title: getLocal("Export Agreements"),
      path: "/myServices?navTo=exportAgreement",
    },
    {
      title: getLocal("Agreement Detail"),
      path: "#",
    },
  ];
  return (
    <>
      <div className="paddedMainDiv">
        <CommonBreadcrumb links={breadcrumbLinks()} />
      </div>
      <Grid container>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={10} md={10}>
          <ExportAgreementDetail
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
        </Grid>
        <Grid item xs={1} md={1}></Grid>
      </Grid>
    </>
  );
}
