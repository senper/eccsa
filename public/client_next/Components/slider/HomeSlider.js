import { Box, Grid, Typography } from '@mui/material';
import Link from 'next/link';
import React from 'react';
import { Carousel, Container } from 'react-bootstrap';
import { getNewsForSlider } from '../../api/news';
import { getSettingValue, getSettingValues } from '../../api/advert';

export default function HomeSlider({ getLocal, getLocalByVal }) {
  const images = [
    {
      url: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse1.mm.bing.net%2Fth%3Fid%3DOIP.1d6tBbNiJTFQNEK_k0sSjQHaFj%26pid%3DApi%26h%3D160&f=1&ipt=df11fcd89dc822ad38a49afb97bc325a524c5c3019448382ebcd0dc34c408656&ipo=images',
    },
    {
      url: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse1.mm.bing.net%2Fth%3Fid%3DOIP.NbfPECA64xbFnmW58MbWDQHaEo%26pid%3DApi%26h%3D160&f=1&ipt=1f5447bf700f124dbb23087ed2fa3d3fcfd88fe239c93dc340fe9f3554cb56a9&ipo=images',
    },
    {
      url: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse1.mm.bing.net%2Fth%3Fid%3DOIP.avb9nDfw3kq7NOoP0grM4wHaEK%26pid%3DApi%26h%3D160&f=1&ipt=10b1acb68b768db6488ee1ead852cd95d69504fbc8d162766c15c7b2df4eb30c&ipo=images',
    },
  ];

  const texts = ['Text 1', 'Text 2', 'Text 3'];

  const [selectedImageIndex, setSelectedImageIndex] = React.useState(0);

  const [textColor, setTextColor] = React.useState('black');
  const [sliderWidth, setSliderWidth] = React.useState('50');

  const handleImageChange = (selectedIndex) => {
    setSelectedImageIndex(selectedIndex);
  };

  const [news, setNews] = React.useState([]);

  React.useEffect(() => {
    getNewsForSlider().then((data) => {
      setNews(data.data.$values);
    });
    getSettingValues(['home_slider_width', 'home_slider_text_color']).then(
      (data) => {
        data.data.$values.forEach((item) => {
          if (item.key === 'home_slider_width') {
            setSliderWidth(item.value);
          } else if (item.key === 'home_slider_text_color') {
            setTextColor(item.value);
          }
        });
      }
    );
    // getSettingValue('home_slider_text_color').then((data) => {
    //   setTextColor(data.data);
    // });

    const handleResize = () => {
      if (window.innerWidth <= 768) {
        setSliderWidth('100');
      } else {
        setSliderWidth('50');
      }
    };
    window.addEventListener('resize', handleResize);
    handleResize();
    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []);

  const imgurl = (id, extension) => {
    const idStr = id.toString();
    const name = idStr.substr(idStr.length - 15).replace('-', '');
    return `${process.env.NEXT_PUBLIC_API_URL}/pr/documents/home/${name}${extension}`;
  };
  const ImageCarousel = ({ images, selectedImageIndex, onImageChange }) => {
    return (
      <div
        className="image-carousel-container"
        style={{
          width: sliderWidth + '%',
          margin: '0 auto',
        }}
      >
        <Carousel
          activeIndex={selectedImageIndex}
          onSelect={onImageChange}
          interval={4000}
          wrap={true}
          slide={true}
          // style={{ backgroundColor: 'black' }}
        >
          {images
            ? images.map((image, index) => (
                <Carousel.Item key={index}>
                  <img
                    src={imgurl(image.id, image.extensionHome)}
                    alt={image.title}
                    className="d-block w-100"
                  />
                </Carousel.Item>
              ))
            : null}
        </Carousel>
      </div>
    );
  };

  const TextCarousel = ({ texts, selectedTextIndex }) => {
    return (
      <div>
        <Carousel
          activeIndex={selectedTextIndex}
          controls={false}
          indicators={false}
          interval={4000}
          wrap={true}
          slide={true}
          className="text-carousel"
        >
          {texts
            ? texts.map((text, index) => (
                <Carousel.Item
                  key={index}
                  style={{ textAlign: 'center' }}
                  className="slider-caption"
                >
                  <Box bgcolor="white" p={2}>
                    <Link href={`/news/detail?news_id=${text.id}`} passHref={true} legacyBehavior>
                      <Typography
                        variant="h5"
                        color={textColor ?? 'black'}
                        style={{
                          cursor: 'pointer',
                        }}
                      >
                        {getLocalByVal(text.title, text.titleLocal)}
                      </Typography>
                      {/* <h5>{getLocalByVal(text.title, text.titleLocal)}</h5> */}
                    </Link>
                  </Box>
                </Carousel.Item>
              ))
            : null}
        </Carousel>
      </div>
    );
  };
  return (
    <Grid item xs={12} md={12}>
      {/* 
      <Carousel>
        {news
          ? news.map((newsValue, index) => (
              <Carousel.Item key={index}>
                {newsValue.homeBannerSetted ? (
                  <img
                    className="d-block w-100"
                    src={imgurl(newsValue.id, newsValue.extensionHome)}
                    alt={newsValue.title}
                  />
                ) : null}
                <Carousel.Caption className="slider-caption">
                  <Link
                    href={`/news/detail?news_id=${newsValue.id}`}
                    passHref={true}
                  >
                    <a className="text-primary" style={{ cursor: 'pointer' }}>
                      <h5>
                        {getLocalByVal(newsValue.title, newsValue.titleLocal)}
                      </h5>
                    </a>
                  </Link>
                </Carousel.Caption>
              </Carousel.Item>
            ))
          : null}
      </Carousel> */}
      <>
        <div className="image-carousel-wrapper">
          <ImageCarousel
            images={news}
            selectedImageIndex={selectedImageIndex}
            onImageChange={handleImageChange}
          />
        </div>
        <>
          <TextCarousel texts={news} selectedTextIndex={selectedImageIndex} />
        </>
      </>
    </Grid>
  );
}
