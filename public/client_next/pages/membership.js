import { Container, Grid } from '@mui/material';
import { useRouter } from 'next/router';
import React from 'react';
import { getAssociation, getECCSAAssociations } from '../api/membership';
import { CommonBreadcrumb } from '../Components/Layout/CommonBreadcrumb';
import AssociationInfoCard from '../Components/Membership/AssociationInfoCard';
import AssociationLinks from '../Components/Membership/AssociationLinks';
import { getLocalByKey } from './api/menu';
import { getServiceOwner } from './api/servicetype';

export default function Membership({ translations, membership, association }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == 'en' || !local ? eng : local;
  };

  const [selectedAssociation, setSelectedAssociation] = React.useState(null);
  const [loadingSelectedAssDetail, setLoadingSelectedAssDetail] =
    React.useState(false);
  const loadAssociation = (id) => {
    setLoadingSelectedAssDetail(true);
    getAssociation(id)
      .then((data) => {
        var res = data.data;
        setSelectedAssociation({
          ...res.association,
          logo: res.logo,
          boardChairman: res.boardChairman,
          ceo: res.ceo,
        });
        setLoadingSelectedAssDetail(false);
      })
      .catch((error) => {
        setLoadingSelectedAssDetail(false);
      });
  };
  const [eccsaAssociations, setECCSAAssociations] = React.useState([]);
  const [loadingList, setLoadingList] = React.useState(false);
  React.useEffect(() => {
    setLoadingList(true);
    setECCSAAssociations(association);
    setLoadingList(false);
  }, []);

  const [membershipDescription, setMembershipDescription] =
    React.useState(null);
  React.useEffect(() => {
    setMembershipDescription(membership);
  }, []);

  const breadcrumbLinks = () => [
    {
      title: getLocal('Home'),
      path: '/',
    },
    {
      title: getLocal('Membership'),
      path: '#',
    },
  ];
  return (
    <>
      <div className="paddedMainDiv">
        <CommonBreadcrumb links={breadcrumbLinks()} />
      </div>
      <Grid container>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={8}>
          <Container>
            {selectedAssociation && !loadingSelectedAssDetail ? (
              <>
                <AssociationInfoCard
                  association={selectedAssociation}
                  getLocal={getLocal}
                  getLocalByVal={getLocalByVal}
                />
                <br />
              </>
            ) : null}
            {loadingSelectedAssDetail ? (
              <strong>{getLocal('Loading Member Detail')} ...</strong>
            ) : null}
            <Grid container spacing={1}>
              <Grid item xs={12}>
                <hr style={{ width: '100%', margin: '0', marginTop: '5px' }} />
                <strong>{getLocal('Membership')}</strong>
                {membershipDescription ? (
                  <pre>
                    {getLocalByVal(
                      membershipDescription.description,
                      membershipDescription.descriptionLocal
                    )}
                  </pre>
                ) : null}
              </Grid>
            </Grid>
          </Container>
        </Grid>
        <Grid item xs={12} md={3}>
          <AssociationLinks
            associations={eccsaAssociations}
            loadAssociation={loadAssociation}
            selectedAssociation={selectedAssociation}
            loadingList={loadingList}
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
        </Grid>
      </Grid>
    </>
  );
}
export async function getServerSideProps(context) {
  let membership = null;
  let association = [];
  await getServiceOwner(1).then((data) => {
    membership = data.data;
  });
  await getECCSAAssociations().then((data) => {
    association = data.data.$values;
  });
  return {
    props: {
      membership,
      association,
    },
  };
}
