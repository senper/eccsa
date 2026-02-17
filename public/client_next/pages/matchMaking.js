import { Grid } from "@mui/material";
import { useRouter } from "next/router";
import React from "react";
import {
  getSectorsList,
  getSourcesList,
  getUserMatches,
  searchSourcesList,
} from "../api/businessInterest";
import { CommonBreadcrumb } from "../Components/Layout/CommonBreadcrumb";
import MMCompanyList from "../Components/MatchMaking/MMCompanyList";
import MMSearchForm from "../Components/MatchMaking/MMSearchForm";
import MMSectorsList from "../Components/MatchMaking/MMSectorsList";
import { getLookupValues } from "./api/contactUs";
import { getLocalByKey } from "./api/menu";

export default function MatchMaking({ translations }) {
  const location = useRouter();
  const { locale } = location;
  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  const tokenMain =
    typeof window !== "undefined" ? localStorage.getItem("access_token") : null;
  const [userMatches, setUserMatches] = React.useState([]);
  React.useEffect(() => {
    loadUserMatches();
  }, [tokenMain]);
  function loadUserMatches() {
    if (tokenMain) {
      getUserMatches().then((data) => {
        setUserMatches(data.data.$values);
      });
    }
  }
  const [searchKey, setSearchKey] = React.useState("");
  const [selectedBusinessType, setSelectedBusinessType] = React.useState(null);
  const [businessTypes, setBusinessTypes] = React.useState([]);
  React.useEffect(() => {
    getLookupValues("00000000-0000-0000-0000-00000000002e").then((data) => {
      setBusinessTypes(data.data.$values);
      if (data.data.$values && data.data.$values.length > 0) {
        setSelectedBusinessType("00000000-0000-1000-0000-000000000022");
      }
    });
  }, []);
  const [searched, setSearched] = React.useState(false);
  const [sectors, setSectors] = React.useState([]);
  const [counts, setCounts] = React.useState([]);
  const [companies, setCompanies] = React.useState([]);
  const [loading, setLoading] = React.useState(false);
  const [selectedSector, setSelectedSector] = React.useState(null);
  React.useEffect(() => {
    getSectorsList().then((data) => {
      setSectors(data.data.sectors.$values);
      setCounts(data.data.counts.$values);
    });
  }, []);
  let sectorIdValue = 0;
  let subSectorIdValue = null;
  if (location.query) {
    const { sectorId, subSectorId } = location.query;
    sectorIdValue = sectorId;
    subSectorIdValue = subSectorId;
  }
  React.useEffect(() => {
    if (subSectorIdValue && sectors.find((s) => s.id == subSectorIdValue)) {
      loadDirectory(sectors.find((s) => s.id == subSectorIdValue));
    }
  }, [subSectorIdValue, sectors]);
  function loadDirectory(sector) {
    setSelectedSector(sector);
    setSearchKey("");
    setSearched(false);
    setCompanies([]);
    setLoading(true);
    getSourcesList(sector.id)
      .then((data) => {
        setCompanies(data.data.$values);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  }
  function searchSubmit(event) {
    event.preventDefault();
    setSelectedSector(null);
    setSearched(false);
    if (!searchKey || searchKey.trim().length <= 0) {
      return;
    }
    setCompanies([]);
    setLoading(true);
    searchSourcesList(selectedBusinessType, searchKey)
      .then((data) => {
        setCompanies(data.data.$values);
        setSearched(true);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  }

  const breadcrumbLinks = () => [
    {
      title: getLocal("Home"),
      path: "/",
    },
    {
      title: getLocal("Match Making"),
      path: "#",
    },
  ];
  return (
    <>
      <div className="paddedMainDiv">
        <CommonBreadcrumb links={breadcrumbLinks()} />
      </div>
      <Grid container spacing={2}>
        <Grid item xs={12} md={3}>
          <MMSectorsList
            sectors={sectors}
            loadDirectory={loadDirectory}
            selectedSector={selectedSector}
            counts={counts}
            sectorId={sectorIdValue}
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
        </Grid>
        <Grid item xs={12} md={9}>
          <MMSearchForm
            searchSubmit={searchSubmit}
            setSearchKey={setSearchKey}
            searchKey={searchKey}
            selectedBusinessType={selectedBusinessType}
            setSelectedBusinessType={setSelectedBusinessType}
            businessTypes={businessTypes}
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
          {loading ? <h3>{getLocal("Loading")}...</h3> : null}
          {!loading && !selectedSector && !searched ? (
            <>
              <hr style={{ width: "100%" }} />
              <h3>{getLocal("Select Sector to get list of Companies")}</h3>
            </>
          ) : null}
          {!loading ? (
            <MMCompanyList
              selectedSector={selectedSector}
              sectors={sectors}
              companies={companies}
              loading={loading}
              businessTypes={businessTypes}
              loadUserMatches={loadUserMatches}
              userMatches={userMatches}
              searched={searched}
              getLocal={getLocal}
              getLocalByVal={getLocalByVal}
            />
          ) : null}
        </Grid>
      </Grid>
    </>
  );
}
