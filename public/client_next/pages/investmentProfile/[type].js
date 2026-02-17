import React from 'react';

import { Container, Grid } from '@mui/material';
import { useRouter } from 'next/router';
import InvestmentProfileLinks from '../../Components/InvestmentProfile/InvestmentProfileLinks';
import InvestmentProfileDetail from '../../Components/InvestmentProfile/InvestmentProfileDetail';
import {
  getInvestmentProfile,
  getInvestmentProfilesList,
} from '../../api/businessInformation';
import { getLocalByKey } from '../api/menu';
import { CommonBreadcrumb } from '../../Components/Layout/CommonBreadcrumb';
import Custom404 from '../404';

const EthiopiaId = '00000000-0000-0000-0000-0000000f4240';
export default function InvestmentProfile({ translations, investmentProfile }) {
  const location = useRouter(); ///
  const { locale } = location;

  const { type } = location.query;
  let typeIdValue = 0;
  if (type) {
    if (type.toLowerCase() == 'ethiopia') {
      typeIdValue = 3;
    } else if (type.toLowerCase() == 'regions') {
      typeIdValue = 1;
    } else if (type.toLowerCase() == 'sectors') {
      typeIdValue = 2;
    }
  }

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == 'en' || !local ? eng : local;
  };
  const breadcrumbLinks = () => [
    {
      title: getLocal('Home'),
      path: '/',
    },
    {
      title: getLocal('Investment Profile'),
      path: '#',
    },
  ];

  React.useEffect(() => {
    if (typeIdValue) {
      changeType(typeIdValue);
    }
  }, [typeIdValue]);

  const [loading, setLoading] = React.useState(false);
  const [profileType, setProfileType] = React.useState(null);

  const [selectedInvestmentProfile, setSelectedInvestmentProfile] =
    React.useState(null);

  const loadInvestmentProfile = (id) => {
    setLoading(true);
    setSelectedInvestmentProfile(investmentProfiles.find((s) => s.id == id));
    getInvestmentProfile(id)
      .then((data) => {
        setSelectedInvestmentProfile(data.data);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  };

  const [investmentProfiles, setInvestmentProfiles] = React.useState([]);

  const [filteredList, setFilteredList] = React.useState([]);
  React.useEffect(() => {
    setInvestmentProfiles(investmentProfile);
    const list = getFilteredList(
      investmentProfile,
      profileType ? profileType : typeIdValue
    );

    setFilteredList([...list]);
    if (list && list.length > 0) {
      loadInvestmentProfile(list[0].id);
    }
  }, []);

  function getProfileListLabel(typeId) {
    return typeId == 3 ? 'Ethiopia' : typeId == 1 ? 'Regions' : 'Sectors';
  }

  function changeType(type) {
    setProfileType(type);
    const list = getFilteredList(investmentProfiles, type);
    setFilteredList([...list]);
    if (list.length > 0) {
      loadInvestmentProfile(list[0].id);
    }
  }
  function getFilteredList(mainList, type) {
    const list = mainList
      ? mainList.filter(
          (l) =>
            (type == 3 && l.id == EthiopiaId) ||
            (type == 1 && l.id != EthiopiaId && l.isRegion) ||
            (type == 2 && l.id != EthiopiaId && !l.isRegion)
        )
      : [];
    return list;
  }

  const MainProfileLink = ({ typeId }) => {
    return (
      <>
        <a
          className="text-primary"
          style={{ cursor: 'pointer' }}
          onClick={() => changeType(typeId)}
        >
          {getLocal(`Change to ${getProfileListLabel(typeId)}`)}
        </a>
        <br />
      </>
    );
  };

  if (!type || !typeIdValue) {
    return <Custom404 />;
  }

  return (
    <>
      <div className="paddedMainDiv">
        <CommonBreadcrumb links={breadcrumbLinks()} />
      </div>
      <Grid container spacing={2}>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={8}>
          <Container>
            {loading ? <h3>{getLocal('Loading')} ...</h3> : null}
            {!loading && selectedInvestmentProfile ? (
              <InvestmentProfileDetail
                profileEntity={selectedInvestmentProfile}
                getLocal={getLocal}
                getLocalByVal={getLocalByVal}
              />
            ) : null}
          </Container>
        </Grid>
        <Grid item xs={12} md={3}>
          {profileType != 3 ? <MainProfileLink typeId={3} /> : null}
          {profileType != 1 ? <MainProfileLink typeId={1} /> : null}
          {profileType != 2 ? <MainProfileLink typeId={2} /> : null}
          <InvestmentProfileLinks
            investmentProfiles={filteredList}
            loadInvestmentProfileDetail={loadInvestmentProfile}
            selectedInvestmentProfile={selectedInvestmentProfile}
            profileListTitle={getProfileListLabel(profileType)}
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
        </Grid>
      </Grid>
    </>
  );
}
export async function getServerSideProps(context) {
  let investmentProfile = [];
  await getInvestmentProfilesList().then((data) => {
    investmentProfile = data.data.$values;
  });

  return {
    props: {
      investmentProfile,
    },
  };
}
